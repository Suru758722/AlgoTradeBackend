using Newtonsoft.Json;
using SudhirTest.Data;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IMarketService
    {
        Task<dynamic> SaveMarketDataAsync();
        Task<dynamic> LoadXTSData();
    }
    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public MarketService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<dynamic> SaveMarketDataAsync()
        {
            try
            {
                const string MARKET_APPKEY = "e16c965046e51516c3e171";
                const string MARKET_SECRET = "Hbui010#Oq";
                var payload = new
                {
                    appKey = MARKET_APPKEY,
                    secretKey = MARKET_SECRET,
                    source = "WEBAPI"
                };
                _httpClient.BaseAddress = new Uri("https://xts.compositedge.com");
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await _httpClient.PostAsync("https://xts.compositedge.com/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(str);
                CancellationToken cancellationToken = new CancellationToken();
                await PeriodicFooAsync(loginResponse.Result.token, _httpClient, cancellationToken);
                return true;
            }catch(Exception ex)
            {
                return ex;
            }

        }
        private async Task PeriodicFooAsync(string token,HttpClient _httpClient,CancellationToken cancellationToken)
        {
            _httpClient.DefaultRequestHeaders.Add("authorization", token);

            while (true)
            {
                await Task.Delay(60000, cancellationToken);
                await GetBinanceApi(token,_httpClient);
            }
        }
        private async Task GetBinanceApi(string token,HttpClient _httpClient)
        {
            try
            {
                string startTime = DateTime.Now.AddMinutes(-2).ToString("MMM dd yyyy HHmmss");
                string endTime = DateTime.Now.ToString("MMM dd yyyy HHmmss");

                HttpResponseMessage Res = await _httpClient.GetAsync("https://xts.compositedge.com/marketdata/instruments/ohlc?exchangeSegment=1&exchangeInstrumentID=22&startTime="+startTime+"&endTime="+endTime+"&compressionValue=60");
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    SymbolModel info = JsonConvert.DeserializeObject<SymbolModel>(response);
                    SymbolData symbol = new SymbolData();
                    symbol.SymbolId = 1;
                    symbol.Price = Convert.ToDouble(info.Result.DataReponse.Split("|")[4]);
                    symbol.Time = DateTime.Now;
                    await _context.SymbolData.AddAsync(symbol);
                    await _context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
            }
            
        }

        public async Task<dynamic> LoadXTSData()
        {
            try
            {
                const string URL = "https://xts.compositedge.com/";

                const string MARKET_APPKEY = "b1329cedc2fb6704cec753";
                const string MARKET_SECRET = "Qpac026$oC";
                var payload = new
                {
                    appKey = MARKET_APPKEY,
                    secretKey = MARKET_SECRET,
                    source = "WebAPI"
                };
                //if (Uri.TryCreate(URL, UriKind.Absolute, out Uri result))
                //{
                //    string authority = result.GetLeftPart(UriPartial.Authority);
                //    _httpClient.BaseAddress = new Uri(authority);
                //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //}
                 _httpClient.BaseAddress = new Uri(URL);
                //_httpClient.DefaultRequestHeaders.Clear();
               // _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = await _httpClient.PostAsync("https://xts.compositedge.com/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")).ConfigureAwait(false);


                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    var info = JsonConvert.DeserializeObject(response);
                }
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
