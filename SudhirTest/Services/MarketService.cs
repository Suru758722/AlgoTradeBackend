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
using SocketIOClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SudhirTest.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SudhirTest.Services
{
    public interface IMarketService
    {
        Task<dynamic> SaveMarketDataAsync();
    }
    public class MarketService : IMarketService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public MarketDataPorts MarketDataPorts { get; set; } = MarketDataPorts.marketDepthEvent;

        public MarketService(IConfiguration config,IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            _scopeFactory = scopeFactory;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<dynamic> SaveMarketDataAsync()
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
                var response = await _httpClient.PostAsync(url + "/apimarketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(str);
                CancellationToken cancellationToken = new CancellationToken();
                await GetSocketData(url, loginResponse.Result.token, _httpClient, cancellationToken);
                return true;
            }catch(Exception ex)
            {
                return ex;
            }

        }
        private async Task GetSocketData(string Url, string token,HttpClient _httpClient,CancellationToken cancellationToken)
        {
            _httpClient.DefaultRequestHeaders.Add("authorization", token);
            string USER_ID = "JHS04";
           var socket = new SocketIO(Url, new SocketIOOptions
            {
                EIO = 3,
                Path = "/apimarketdata/socket.io",
                Query = new Dictionary<string, string>()
                    {
                        { "token", token },
                        { "userID", USER_ID },
                        { "source", "WebAPI" },
                        { "publishFormat", "JSON" },
                        { "broadcastMode", "Full" }
                    }
            });
            
            socket.On("1502-json-full", response =>
            {

                var obj = response.GetValue();

                var mdp = JsonConvert.DeserializeObject<MdpModel>(obj.ToString(), new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });
               
                StoreData(mdp);
               
            });

            await socket.ConnectAsync();
            await SubscribeAsync();
        }
        private void StoreData(MdpModel mdp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string instrumentName = Enum.GetName(typeof(InstrumentNumber), Convert.ToInt32(mdp.ExchangeInstrumentID));
            DateTime currentTime = dateTime.AddSeconds(mdp.LastTradedTime);
            DateTime tableTime;
            string sql = "Select * top 1 From" + instrumentName + "order by Id Desc";
            using (SqlConnection conn = new SqlConnection(_config["ConnectionStrings:connection"]))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    DataTable data = new DataTable();
                    data.Load(command.ExecuteReader());

                   // List<InsertDataModel> table = data;// .ToList<InsertDataModel>();
                   
                }
                if (tableTime.AddMinutes(1).ToString("HH:mm") == currentTime.ToString("HH:mm"))
                {
                    using (SqlCommand command = new SqlCommand("insert into"+ instrumentName+"values("+
                            mdp.LastTradedPrice + ","+ mdp.LastTradedTime + ","+
                            mdp.ExchangeInstrumentID+","+mdp.LastTradedQunatity+");", conn))
                             {
                                 command.ExecuteNonQuery();
                             }
                }
                conn.Close();

            }
            
        }
        private async Task SubscribeAsync()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1502
            };

            var response = await _httpClient.PostAsync(@"/apimarketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

            string txt;

            if (response != null)
            {
                txt = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }


        }
        private List<Instruments> GetInstruments(MarketDataPorts port)
        {

            //int exchange = (int)ExchangeSegment.MCXFO;
            int exchange = 1;
            long exchangeInstrumentId = 22;   //reliance

            //if (this.MarketDataPorts == MarketDataPorts.openInterestEvent)
            //{
            //    exchange = (int)ExchangeSegment.NSEFO;
            //    exchangeInstrumentId = 45042; //nifty sep 19 fut
            //}
            //else if (this.MarketDataPorts == MarketDataPorts.indexDataEvent)
            //{
            //    exchangeInstrumentId = 1;
            //}
            List<Instruments> list = new List<Instruments>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var temp = db.Instrument.Include(x => x.ExchangeSegment).Select(x => new Instruments {exchangeSegment = (int)x.ExchangeSegment.ExchangeId ,exchangeInstrumentID = x.ExchangeInstrumentID});
                list = temp.ToList()
;            }

            return list;

        }
       
    }
}
