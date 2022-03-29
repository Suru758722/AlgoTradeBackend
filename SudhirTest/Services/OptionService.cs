using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using SocketIOClient;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IOptionService
    {
        Task<dynamic> SaveOptionData();
    }
    public class OptionService : IOptionService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;       
        public MarketDataPorts MarketDataPorts { get; set; } = MarketDataPorts.marketDepthEvent;

        public OptionService(IConfiguration config, IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            _scopeFactory = scopeFactory;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<dynamic> SaveOptionData()
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

            socket.On("1501-json-partial", response =>
            {
                var obj = response.GetValue();
                string mdp = obj.ToString();

                Dictionary<string, string> keyValuePairs = mdp.Split(',')
                    .Select(value => value.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
                var dico = keyValuePairs.ToList();
                StoreData(Convert.ToInt32(dico.Where(x => x.Key == "t").FirstOrDefault().Value.Split("_")[1]), Convert.ToInt64(dico.Where(x => x.Key == "ltt").FirstOrDefault().Value), Convert.ToDouble(dico.Where(x => x.Key == "ltp").FirstOrDefault().Value), Convert.ToInt64(dico.Where(x => x.Key == "ltq").FirstOrDefault().Value));
            });
            socket.On("1510-json-full", response =>
            {

                var obj = response.GetValue();

                var mdp = JsonConvert.DeserializeObject<OpenInterestModel>(obj.ToString(), new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });

              //  StoreData(mdp.ExchangeInstrumentID, mdp.LastTradedTime, mdp.LastTradedPrice, mdp.LastTradedQunatity);

            });

            await socket.ConnectAsync();
            await SubscribeAsync();
        }
        private void StoreData(int ExchangeInstrumentID, long LastTradedTime, double LastTradedPrice, long LastTradedQunatity)
        {
            // DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string instrumentName = Enum.GetName(typeof(InstrumentNumberEnum), Convert.ToInt32(ExchangeInstrumentID));
            //DateTime currentTime = new DateTime(LastTradedTime);  //dateTime.AddSeconds(Math.Round(LastTradedTime / 1000d)).ToLocalTime();
            //DateTime tableTime;
            DataTable dataTable = new DataTable();
            List<InsertDataModel> list = new List<InsertDataModel>();
            string sql = "select * from " + instrumentName.ToLower() + " order by id desc fetch first 1 rows only";
            using (var con = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
            {
                con.Open();
                using (var cmd = new NpgsqlCommand(sql, con))
                {

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        list.Add(new InsertDataModel { LastTradedPrice = (double)dr.ItemArray[1], LastTradedTime = (long)dr.ItemArray[2] });
                    }

                }
                if (list.Count == 0)
                {
                    string sqlQuery = "insert into " + instrumentName.ToLower() + "(lasttradedprice,lasttradedtime,exchangeinstrumentid,lasttradedqunatity) values(" +
                            LastTradedPrice + "," + LastTradedTime + "," +
                            ExchangeInstrumentID + "," + LastTradedQunatity + ")";
                    using (var command = new NpgsqlCommand(sqlQuery, con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string current = UnixTimeStampToDateTime(LastTradedTime).ToString("HH:mm");
                    string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                    if (current != previous)
                    {
                        string sqlQuery = "insert into " + instrumentName.ToLower() + "(lasttradedprice,lasttradedtime,exchangeinstrumentid,lasttradedqunatity) values(" +
                                LastTradedPrice + "," + LastTradedTime + "," +
                                ExchangeInstrumentID + "," + LastTradedQunatity + ")";
                        using (var command = new NpgsqlCommand(sqlQuery, con))
                        {
                            command.ExecuteNonQuery();
                        }
                        
                    }
                }
                con.Close();

            }

        }
        private async Task SubscribeAsync()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1510
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
            //int exchange = 1;
            //long exchangeInstrumentId = 22;   //reliance

            //if (this.MarketDataPorts == MarketDataPorts.openInterestEvent)
            //{
            //    exchange = (int)ExchangeSegmentEnum.NSEFO;
            //    exchangeInstrumentId = 45042; //nifty sep 19 fut
            //}
            //else if (this.MarketDataPorts == MarketDataPorts.indexDataEvent)
            //{
            //    exchangeInstrumentId = 1;
            //}
            List<Instruments> list = new List<Instruments>();

            foreach (InstrumentNumberEnum val in Enum.GetValues(typeof(OptionNumberEnum)))
            {
                list.Add(new Instruments { exchangeSegment = (int)ExchangeSegmentEnum.NSEFO, exchangeInstrumentID = (long)val });
            }


            return list;

        }
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime.AddYears(10);
        }
    }
}
