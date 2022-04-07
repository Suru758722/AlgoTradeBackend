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
using Npgsql;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;

namespace SudhirTest.Services
{
    public interface IMarketService
    {
        Task<dynamic> SaveMarketDataAsync();
        dynamic TestMethod();
    }
    public class MarketService : IMarketService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "TestDotnet";
        static readonly string sheet = "Sheet1";
        static readonly string SpreadsheetId = "1W0I_bfWuRED_p6pOs6ROCKhS3soMiDsGm2RtPNbTacE";
        static SheetsService service;
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
                var response = await _httpClient.PostAsync(url + "/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
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

            socket.On("1501-json-partial", response =>
            {
                var obj = response.GetValue();
                string mdp = obj.ToString();

                Dictionary<string, string> keyValuePairs = mdp.Split(',')
                    .Select(value => value.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
                var dico = keyValuePairs.ToList();
                  StoreData(Convert.ToInt32(dico.Where(x => x.Key == "t").FirstOrDefault().Value.Split("_")[1]),Convert.ToInt64(dico.Where(x => x.Key == "ltt").FirstOrDefault().Value), Convert.ToDouble(dico.Where(x => x.Key == "ltp").FirstOrDefault().Value), Convert.ToInt64(dico.Where(x => x.Key == "ltq").FirstOrDefault().Value));
            });
                socket.On("1501-json-full", response =>
            {

                var obj = response.GetValue();

                var mdp = JsonConvert.DeserializeObject<ToucheLineModel>(obj.ToString(), new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });
               
                StoreData(mdp.ExchangeInstrumentID,mdp.LastTradedTime,mdp.LastTradedPrice,mdp.LastTradedQunatity);
               
            });

            await socket.ConnectAsync();
            await SubscribeAsync();
        }
        private void StoreData(int ExchangeInstrumentID,long LastTradedTime,double LastTradedPrice,long LastTradedQunatity)
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
                        list.Add(new InsertDataModel { LastTradedPrice = (double)dr.ItemArray[1],LastTradedTime = (long)dr.ItemArray[2]});
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
                        if (instrumentName.ToLower() == InstrumentNumberEnum.HDFC.ToString().ToLower())
                        {
                            AddtoGoogleSheet(LastTradedTime, LastTradedPrice, LastTradedQunatity);
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
                xtsMessageCode = 1501
            };

            var response = await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

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
            return dateTime.AddYears(10);
        }
        public dynamic TestMethod()
        {
            
            return true;
        }
        private void AddtoGoogleSheet(long LastTradedTime, double LastTradedPrice, long LastTradedQunatity)
        {
            GoogleCredential credential;
            //Reading Credentials File...
            string folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"json\\app_client_secret.json"}");
            using (var stream = new FileStream(folderDetails, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Creating Google Sheets API service...
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            AddRow(LastTradedTime, LastTradedPrice, LastTradedQunatity);
        }

        private void AddRow(long LastTradedTime, double LastTradedPrice, long LastTradedQunatity)
        {
            DateTime tradedTime = UnixTimeStampToDateTime(LastTradedTime);
            string range = $"{sheet}!A:C";

            SpreadsheetsResource.ValuesResource.GetRequest request =
            service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();

            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 1200)
            {
                Request RequestBody = new Request()
                {
                    DeleteDimension = new DeleteDimensionRequest()
                    {
                        Range = new DimensionRange()
                        {
                            SheetId = 0,
                            Dimension = "ROWS",
                            StartIndex = 0,
                            EndIndex = 1
                        }
                    }
                };

                List<Request> RequestContainer = new List<Request>();
                RequestContainer.Add(RequestBody);

                BatchUpdateSpreadsheetRequest DeleteRequest = new BatchUpdateSpreadsheetRequest();
                DeleteRequest.Requests = RequestContainer;

                var req = service.Spreadsheets.BatchUpdate(DeleteRequest, SpreadsheetId);
                req.Execute();
            }
           
            var valueRange = new ValueRange();
            var oblist = new List<object>() { tradedTime, LastTradedPrice, LastTradedQunatity };
            valueRange.Values = new List<IList<object>> { oblist };
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }

    }
}
