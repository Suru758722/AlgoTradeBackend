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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IMarketService
    {
        Task<dynamic> SaveMarketDataAsync();
        dynamic TestMethod();
        Task<dynamic> StopStock();

    }
    public class MarketService : IMarketService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public MarketDataPorts MarketDataPorts { get; set; } = MarketDataPorts.marketDepthEvent;

        public MarketService(IConfiguration config, IServiceScopeFactory scopeFactory, HttpClient httpClient)
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
                var response = await _httpClient.PostAsync(url + "/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(str);
                await GetSocketData(url, loginResponse.Result.token, _httpClient, false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private async Task GetSocketData(string Url, string token, HttpClient _httpClient, bool closeSocket)
        {
            _httpClient.DefaultRequestHeaders.Add("authorization", token);
            string USER_ID = "JHS04";
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
            if (!closeSocket)
            {
                socket.On("1501-json-partial", response =>
            {
                var obj = response.GetValue();
                string mdp = obj.ToString();

                Dictionary<string, string> keyValuePairs = mdp.Split(',')
                    .Select(value => value.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
                var dico = keyValuePairs.ToList();
                StoreData(Convert.ToInt32(dico.Where(x => x.Key == "t").FirstOrDefault().Value.Split("_")[1]), Convert.ToInt64(dico.Where(x => x.Key == "ltt").FirstOrDefault().Value), Convert.ToDouble(dico.Where(x => x.Key == "ltp").FirstOrDefault().Value), Convert.ToInt64(dico.Where(x => x.Key == "ltq").FirstOrDefault().Value), 0);
            });
                socket.On("1501-json-full", response =>
            {

                var obj = response.GetValue();

                var mdp = JsonConvert.DeserializeObject<ToucheLineModel>(obj.ToString(), new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });

                StoreData(mdp.ExchangeInstrumentID, mdp.LastTradedTime, mdp.LastTradedPrice, mdp.LastTradedQunatity, 0);

            });

                await socket.ConnectAsync();
                await SubscribeAsync();
            }
            if (closeSocket)
            {
                await socket.DisconnectAsync();
            }
        }
        private void StoreData(int ExchangeInstrumentID, long LastTradedTime, double LastTradedPrice, long LastTradedQunatity, double OI)
        {
            string instrumentName = Enum.GetName(typeof(InstrumentNumberEnum), Convert.ToInt32(ExchangeInstrumentID));
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
                instruments = GetInstruments(),
                xtsMessageCode = 1501
            };

            var response = await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

            string txt;

            if (response != null)
            {
                txt = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }


        }
        private List<Instruments> GetInstruments()
        {


            List<Instruments> list = new List<Instruments>();

            foreach (InstrumentNumberEnum val in Enum.GetValues(typeof(InstrumentNumberEnum)))
            {

                list.Add(new Instruments { exchangeSegment = (int)ExchangeSegmentEnum.NSECM, exchangeInstrumentID = (long)val });

            }


            return list;

        }
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            if (dateTime.Year == 2012)
                return dateTime.AddYears(10);
            else
                return dateTime;
        }
        public long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public dynamic TestMethod()
        {

            return true;
        }
        //private void AddtoGoogleSheet(long LastTradedTime, double LastTradedPrice, long LastTradedQunatity)
        //{
        //    GoogleCredential credential;
        //    //Reading Credentials File...
        //    string folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"json\\app_client_secret.json"}");
        //    using (var stream = new FileStream(folderDetails, FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(stream)
        //            .CreateScoped(Scopes);
        //    }

        //    // Creating Google Sheets API service...
        //    service = new SheetsService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });
        //   // AddRow(LastTradedTime, LastTradedPrice, LastTradedQunatity);
        //}

        //private void AddRow(long LastTradedTime, double LastTradedPrice, long LastTradedQunatity)
        //{
        //    DateTime tradedTime = UnixTimeStampToDateTime(LastTradedTime);
        //    string range = $"{sheet}!A:C";

        //    SpreadsheetsResource.ValuesResource.GetRequest request =
        //    service.Spreadsheets.Values.Get(SpreadsheetId, range);
        //    var response = request.Execute();

        //    IList<IList<object>> values = response.Values;
        //    if (values != null && values.Count > 1200)
        //    {
        //        Request RequestBody = new Request()
        //        {
        //            DeleteDimension = new DeleteDimensionRequest()
        //            {
        //                Range = new DimensionRange()
        //                {
        //                    SheetId = 0,
        //                    Dimension = "ROWS",
        //                    StartIndex = 0,
        //                    EndIndex = 1
        //                }
        //            }
        //        };

        //        List<Request> RequestContainer = new List<Request>();
        //        RequestContainer.Add(RequestBody);

        //        BatchUpdateSpreadsheetRequest DeleteRequest = new BatchUpdateSpreadsheetRequest();
        //        DeleteRequest.Requests = RequestContainer;

        //        var req = service.Spreadsheets.BatchUpdate(DeleteRequest, SpreadsheetId);
        //        req.Execute();
        //    }

        //    var valueRange = new ValueRange();
        //    var oblist = new List<object>() { tradedTime, LastTradedPrice, LastTradedQunatity };
        //    valueRange.Values = new List<IList<object>> { oblist };
        //    var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
        //    appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //    appendRequest.Execute();
        //}

        public async Task<dynamic> StopStock()
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
                await GetSocketData(url, loginResponse.Result.token, _httpClient, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
