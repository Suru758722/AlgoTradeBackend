using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using SocketIOClient;
using SudhirTest.Data;
using SudhirTest.Entity;
using SudhirTest.Model;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
                StoreData(Convert.ToInt32(dico.Where(x => x.Key == "t").FirstOrDefault().Value.Split("_")[1]), Convert.ToInt64(dico.Where(x => x.Key == "ltt").FirstOrDefault().Value), Convert.ToDouble(dico.Where(x => x.Key == "ltp").FirstOrDefault().Value), Convert.ToInt64(dico.Where(x => x.Key == "ltq").FirstOrDefault().Value), 0);

            });
            socket.On("1510-json-partial", response =>
            {

                var obj = response.GetValue().ToString();

               
                Dictionary<string, string> keyValuePairs = obj.Split(',')
                    .Select(value => value.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
                var dico = keyValuePairs.ToList();
                StoreData(Convert.ToInt32(dico.Where(x => x.Key == "t").FirstOrDefault().Value.Split("_")[1]), ToUnixTime(DateTime.Now), 0, 0, Convert.ToDouble(dico.Where(x => x.Key == "oi").FirstOrDefault().Value));

            });
           
            await socket.ConnectAsync();
            await SubscribeAsync();
        }
        private void StoreData(int ExchangeInstrumentID, long LastTradedTime, double LastTradedPrice, long LastTradedQunatity,double OI)
        {
            string current = UnixTimeStampToDateTime(LastTradedTime).ToString("HH:mm");

            using (var con = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
            {
                con.Open();
                DataTable dataTable = new DataTable();
                List<OptionInsertModel> optionInsertModel = new List<OptionInsertModel>();
                string sqlCheckQuery = "select * from optioninstrument where exchangeinstrumentid=" + ExchangeInstrumentID;


                using (var cmd = new NpgsqlCommand(sqlCheckQuery, con))
                {

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        optionInsertModel.Add(new OptionInsertModel { OptionType = dr.ItemArray[5].ToString(), StrikePrice = (int)dr.ItemArray[2] });
                    }

                }
                if (optionInsertModel.FirstOrDefault().OptionType == "CE")
                {
                    if (OI == 0)
                    {
                        List<OptionInsertModel> list = new List<OptionInsertModel>();
                        string sql = "select * from optionltpcall where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable1 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable1);
                            foreach (DataRow dr in dataTable1.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[1] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionltpcall(time,exchangeinstrumentid,ltp,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedPrice+","+ optionInsertModel.FirstOrDefault().StrikePrice+","+UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionltpcall(time,exchangeinstrumentid,ltp,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedPrice + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                        list = new List<OptionInsertModel>();
                        sql = "select * from optionltqcall where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable2 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable2);
                            foreach (DataRow dr in dataTable2.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionltqcall(time,exchangeinstrumentid,ltq,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedQunatity + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionltqcall(time,exchangeinstrumentid,ltq,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedQunatity + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                    else
                    {
                        List<OptionInsertModel> list = new List<OptionInsertModel>();

                        list = new List<OptionInsertModel>();
                        string sql = "select * from optionoicall where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable3 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable3);
                            foreach (DataRow dr in dataTable3.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionoicall(time,exchangeinstrumentid,oi,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + OI + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionoicall(time,exchangeinstrumentid,oi,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + OI + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (OI == 0)
                    {
                        List<OptionInsertModel> list = new List<OptionInsertModel>();
                        string sql = "select * from optionltpput where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable4 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable4);
                            foreach (DataRow dr in dataTable4.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionltpput(time,exchangeinstrumentid,ltp,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedPrice + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionltpput(time,exchangeinstrumentid,ltp,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedPrice + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                        list = new List<OptionInsertModel>();
                        sql = "select * from optionltqput where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable5 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable5);
                            foreach (DataRow dr in dataTable5.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionltqput(time,exchangeinstrumentid,ltq,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedQunatity + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionltqput(time,exchangeinstrumentid,ltq,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + LastTradedQunatity + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                    else
                    {
                        List<OptionInsertModel> list = new List<OptionInsertModel>();

                        list = new List<OptionInsertModel>();
                        string sql = "select * from optionoiput where exchangeinstrumentid=" + ExchangeInstrumentID + " order by id desc fetch first 1 rows only";


                        using (var cmd = new NpgsqlCommand(sql, con))
                        {
                            DataTable dataTable6 = new DataTable();

                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dataTable6);
                            foreach (DataRow dr in dataTable6.Rows)
                            {
                                list.Add(new OptionInsertModel { LastTradedTime = (long)dr.ItemArray[2] });
                            }

                        }

                        if (list.Count == 0)
                        {
                            string sqlQuery = "insert into optionoiput(time,exchangeinstrumentid,oi,strikeprice,timestring) values(" +
                                    LastTradedTime + "," + ExchangeInstrumentID + "," + OI + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string previous = UnixTimeStampToDateTime(list.FirstOrDefault().LastTradedTime).ToString("HH:mm");

                            if (current != previous)
                            {
                                string sqlQuery = "insert into optionoiput(time,exchangeinstrumentid,oi,strikeprice,timestring) values(" +
                                      LastTradedTime + "," + ExchangeInstrumentID + "," + OI + "," + optionInsertModel.FirstOrDefault().StrikePrice + "," + UnixTimeStampToDateTime(LastTradedTime).ToString("dd/MM/yyyy HH:mm") + ")";
                                using (var command = new NpgsqlCommand(sqlQuery, con))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
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

             await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

             payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1501
            };
            await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

        }
        private List<Instruments> GetInstruments(MarketDataPorts port)
        {

            List<Instruments> list = new List<Instruments>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var optionList = db.OptionInstrument.ToList();
                foreach (var element in optionList)
                {
                    if (Convert.ToDateTime(element.Expiry) > DateTime.Now)
                    {
                        list.Add(new Instruments { exchangeSegment = (int)ExchangeSegmentEnum.NSEFO, exchangeInstrumentID = element.ExchangeInstrumentId });
                    }
                    }
            }
            return list;

        }
        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            if (dateTime.Year == 2012)
                return dateTime.AddYears(10);
            else
                return dateTime;
        }
        public  long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}
