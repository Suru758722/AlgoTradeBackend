using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SocketIOClient;
using SudhirTest.Data;
using SudhirTest.Entity;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IIndexService
    {
        Task<dynamic> SaveOptionInstrument();

    }
    public class IndexService : IIndexService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;
        public MarketDataPorts MarketDataPorts { get; set; } = MarketDataPorts.marketDepthEvent;

        public IndexService(IConfiguration config, IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            _scopeFactory = scopeFactory;
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<dynamic> SaveOptionInstrument()
        {
            try
            {
                string url = _config["Xts:BaseUrl"];
                string appKey = _config["Xts:AppKey"];
                string secret = _config["Xts:Secret"];
                var payload = new
                {
                    appKey = appKey,
                    secretKey = secret,
                    source = "WEBAPI"
                };
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await _httpClient.PostAsync(url + "/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(str);
                CancellationToken cancellationToken = new CancellationToken();
                await GetSocketData(url, loginResponse.Result.token, _httpClient, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
        private async Task GetSocketData(string Url, string token, HttpClient _httpClient, CancellationToken cancellationToken)
        {
            _httpClient.DefaultRequestHeaders.Add("authorization", token);
            string USER_ID = "JHS04";
            bool loadOnce = true;
            var socket = new SocketIO(Url, new SocketIOOptions
            {
                EIO = 3,
                Path = "/marketdata/socket.io",
                Query = new Dictionary<string, string>()
                    {
                        { "token", token },
                        { "userID", USER_ID },
                        { "source", "WebAPI" },
                        { "publishFormat", "JSON" },
                        { "broadcastMode", "Partial" }
                    }
            });

            socket.On("1501-json-partial", async response =>
            {
                var obj = response.GetValue();
                string mdp = obj.ToString();

                Dictionary<string, string> keyValuePairs = mdp.Split(',')
                    .Select(value => value.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
                var dico = keyValuePairs.ToList();
                if (loadOnce)
                {
                    loadOnce = false;
                    await SaveStrikePrices(Url, Convert.ToDecimal(dico.Where(x => x.Key == "ltp").FirstOrDefault().Value));
               
                }

               
            });
            

            await socket.ConnectAsync();
            await SubscribeAsync();
        }
        private async Task SubscribeAsync()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1501
            };

            await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

           
        }
        private async Task UnSubscribeAsync()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1501
            };

            await _httpClient.PutAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);


        }
        private List<Instruments> GetInstruments(MarketDataPorts port)
        {

            List<Instruments> list = new List<Instruments>();
            list.Add(new Instruments { exchangeSegment = 1, exchangeInstrumentID = 26000 });
            return list;

        }
        public async Task SaveStrikePrices(string url, decimal niftyPrice)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var list = db.OptionInstrument.ToList();
                if (list.Count > 0)
                {
                    db.OptionInstrument.RemoveRange(list);
                    db.SaveChanges();
                }
            }
            var expiryResponse = await _httpClient.GetAsync(url + "/marketdata/instruments/instrument/expiryDate?exchangeSegment=2&series=OPTIDX&symbol=Nifty").ConfigureAwait(false);
                string strExpiry = await expiryResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                ExpiryModel expiryList = JsonConvert.DeserializeObject<ExpiryModel>(strExpiry);

                expiryList.Result = expiryList.Result.Where(x => Convert.ToDateTime(x).Month == DateTime.Now.Month).Select(x => x).OrderBy(x => Convert.ToDateTime(x)).ToList();

                DateTime expiryDate = Convert.ToDateTime(expiryList.Result.FirstOrDefault());
                string expiryDateString = expiryDate.ToString("dd MMM yyyy").Replace(" ", "");
                List<StrikPriceModel> strikPricesCall = new List<StrikPriceModel>();
                List<StrikPriceModel> strikPricesPut = new List<StrikPriceModel>();


            for (int i = 0; i <= 10; i++)
                {
                strikPricesCall.Add(new StrikPriceModel { OptionType = "CE", StrikePrice = ((int)(niftyPrice / 100)) * 100 + i * 100 });
                }
                for (int i = 0; i <= 10; i++)
                {
                strikPricesCall.Add(new StrikPriceModel { OptionType = "CE", StrikePrice = ((int)(niftyPrice / 100)) * 100 - i * 100 });
                }

                strikPricesPut = strikPricesCall.Select(x => new StrikPriceModel { StrikePrice = x.StrikePrice, OptionType = "PE" }).ToList();
                strikPricesCall.AddRange(strikPricesPut);
                foreach (var element in strikPricesCall)
                {
                    string optionUrl = url + "/marketdata/instruments/instrument/optionSymbol?exchangeSegment=2&series=OPTIDX&symbol=NIFTY&expiryDate=" + expiryDateString + "&optionType=" + element.OptionType + "&strikePrice=" + element.StrikePrice;
                    var strikePriceResponse = await _httpClient.GetAsync(optionUrl).ConfigureAwait(false);

                    string strikePriceResult = await strikePriceResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var strikePriceList = JsonConvert.DeserializeObject<ExpiryOptionModel>(strikePriceResult);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                        var _doesExist = await db.OptionInstrument.Where(x => x.ExchangeInstrumentId == strikePriceList.Result.FirstOrDefault().ExchangeInstrumentId).FirstOrDefaultAsync();

                        if (_doesExist == null)
                        {
                            OptionInstrument optionInstrument = new OptionInstrument();
                            optionInstrument.StrikePrice = element.StrikePrice;
                            optionInstrument.OptionType = element.OptionType;
                            optionInstrument.Expiry = expiryList.Result.FirstOrDefault();
                            optionInstrument.ExchangeInstrumentId = strikePriceList.Result.FirstOrDefault().ExchangeInstrumentId;
                            optionInstrument.Discription = strikePriceList.Result.FirstOrDefault().Description;
                            optionInstrument.DisplayName = strikePriceList.Result.FirstOrDefault().DisplayName;
                            optionInstrument.Symbol = "Nifty";

                            await db.OptionInstrument.AddAsync(optionInstrument);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            _doesExist.StrikePrice = element.StrikePrice;
                            _doesExist.OptionType = element.OptionType;
                            _doesExist.Expiry = expiryList.Result.FirstOrDefault();
                            _doesExist.ExchangeInstrumentId = strikePriceList.Result.FirstOrDefault().ExchangeInstrumentId;
                            _doesExist.Discription = strikePriceList.Result.FirstOrDefault().Description;
                            _doesExist.DisplayName = strikePriceList.Result.FirstOrDefault().DisplayName;
                            _doesExist.Symbol = "Nifty";
                            await db.SaveChangesAsync();
                        }
                    }
                }
            await UnSubscribeAsync();


        }
    }
}
