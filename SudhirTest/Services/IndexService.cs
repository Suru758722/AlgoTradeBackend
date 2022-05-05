using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using SocketIOClient;
using SudhirTest.Data;
using SudhirTest.Entity;
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
    public interface IIndexService
    {
        Task<dynamic> SaveOptionInstrument();
        Task<dynamic> SaveFutureInstrument();
        Task<dynamic> SaveFutureData();
        dynamic DeletePreviousData();
        Task<dynamic> StopFuture();

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
                await GetSocketNiftyData(url, loginResponse.Result.token, _httpClient, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private async Task GetSocketNiftyData(string Url, string token, HttpClient _httpClient, CancellationToken cancellationToken)
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
            await SubscribeAsyncNifty();
        }
        private async Task SubscribeAsyncNifty()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstrumentNifty(MarketDataPorts),
                xtsMessageCode = 1501
            };

            await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);


        }
        private async Task UnSubscribeAsyncNifty()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstrumentNifty(MarketDataPorts),
                xtsMessageCode = 1501
            };

            await _httpClient.PutAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);


        }
        private List<Instruments> GetInstrumentNifty(MarketDataPorts port)
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
            await UnSubscribeAsyncNifty();


        }

        public async Task<dynamic> SaveFutureInstrument()
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
                await SaveData(url, loginResponse.Result.token, _httpClient);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task SaveData(string url, string token, HttpClient httpClient)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var list = db.FutureInstrument.ToList();
                if (list.Count > 0)
                {
                    db.FutureInstrument.RemoveRange(list);
                    db.SaveChanges();
                }
            }
            _httpClient.DefaultRequestHeaders.Add("authorization", token);

            var expiryResponse = await httpClient.GetAsync(url + "/marketdata/instruments/instrument/expiryDate?exchangeSegment=2&series=FutIdx&symbol=Nifty").ConfigureAwait(false);
            string strExpiry = await expiryResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            ExpiryModel expiryList = JsonConvert.DeserializeObject<ExpiryModel>(strExpiry);

            expiryList.Result = expiryList.Result.Where(x => Convert.ToDateTime(x).Month == DateTime.Now.Month).Select(x => x).OrderBy(x => Convert.ToDateTime(x)).ToList();

            DateTime expiryDate = Convert.ToDateTime(expiryList.Result.FirstOrDefault());
            string expiryDateString = expiryDate.ToString("dd MMM yyyy").Replace(" ", "");

            string optionUrl = url + "/marketdata/instruments/instrument/futureSymbol?exchangeSegment=2&series=FutIdx&symbol=NIFTY&expiryDate=" + expiryDateString;
            var strikePriceResponse = await httpClient.GetAsync(optionUrl).ConfigureAwait(false);

            string strikePriceResult = await strikePriceResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var strikePriceList = JsonConvert.DeserializeObject<ExpiryOptionModel>(strikePriceResult);

            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();


                FutureInstrument instrument = new FutureInstrument();
                instrument.Expiry = expiryList.Result.FirstOrDefault();
                instrument.ExchangeInstrumentId = strikePriceList.Result.FirstOrDefault().ExchangeInstrumentId;
                instrument.Discription = strikePriceList.Result.FirstOrDefault().Description;
                instrument.DisplayName = strikePriceList.Result.FirstOrDefault().DisplayName;
                instrument.Symbol = "Nifty";

                await db.FutureInstrument.AddAsync(instrument);
                await db.SaveChangesAsync();

            }

        }

        public async Task<dynamic> SaveFutureData()
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
            if (closeSocket)
            {
                await socket.DisconnectAsync();
            }
        }
        private void StoreData(int ExchangeInstrumentID, long LastTradedTime, double LastTradedPrice, long LastTradedQunatity, double OI)
        {
            DataTable dataTable = new DataTable();
            List<InsertDataModel> list = new List<InsertDataModel>();
            string sql = "select * from niftyfut order by id desc fetch first 1 rows only";
            using (var con = new NpgsqlConnection(_config["ConnectionStrings:connection"]))
            {
                con.Open();

                if (OI == 0)
                {

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
                        string sqlQuery = "insert into niftyfut(lasttradedprice,lasttradedtime,exchangeinstrumentid,lasttradedqunatity,oi,timestring) values(" +
                                LastTradedPrice + "," + LastTradedTime + "," +
                                ExchangeInstrumentID + "," + LastTradedQunatity + ",0,'" + UnixTimeStampToDateTime(LastTradedTime).ToString("HH:mm") + "')";
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
                            string sqlQuery = "insert into niftyfut(lasttradedprice,lasttradedtime,exchangeinstrumentid,lasttradedqunatity,oi,timestring) values(" +
                               LastTradedPrice + "," + LastTradedTime + "," +
                               ExchangeInstrumentID + "," + LastTradedQunatity + ",0,'" + UnixTimeStampToDateTime(LastTradedTime).ToString("HH:mm") + "')";
                            using (var command = new NpgsqlCommand(sqlQuery, con))
                            {
                                command.ExecuteNonQuery();
                            }

                        }

                    }

                }
                else
                {

                    string sqlQuery = "update niftyfut set oi=" + OI + " where oi = 0 and timestring='" + UnixTimeStampToDateTime(LastTradedTime).ToString("HH:mm") + "'";
                    using (var command = new NpgsqlCommand(sqlQuery, con))
                    {
                        command.ExecuteNonQuery();
                    }
                }


                con.Close();

            }

        }
        private async Task SubscribeAsync()
        {
            List<Instruments> listOI = new List<Instruments>();

            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var list = db.FutureInstrument.FirstOrDefault();
                listOI.Add(new Instruments { exchangeSegment = (int)ExchangeSegmentEnum.NSEFO, exchangeInstrumentID = list.ExchangeInstrumentId });

            }


            SubscriptionPayload payload1 = new SubscriptionPayload()
            {

                instruments = listOI,
                xtsMessageCode = 1510
            };

            await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload1?.GetHttpContent()).ConfigureAwait(false);

            SubscriptionPayload payload2 = new SubscriptionPayload()
            {
                instruments = listOI,
                xtsMessageCode = 1501
            };

            var response = await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload2?.GetHttpContent()).ConfigureAwait(false);

            string txt;

            if (response != null)
            {
                txt = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }


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

        public dynamic DeletePreviousData()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var listcallltp = db.OptionLtpCall.ToList();

                    if (listcallltp.Count > 0)
                    {
                        var weeklyList = listcallltp.Select(x => new OptionLtpCallWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            Ltp = x.Ltp,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionLtpCallWeekly.AddRange(weeklyList);
                        db.OptionLtpCall.RemoveRange(listcallltp);
                        db.SaveChanges();
                    }
                    var listputltp = db.OptionLtpPut.ToList();

                    if (listputltp.Count > 0)
                    {
                        var weeklyList = listputltp.Select(x => new OptionLtpPutWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            Ltp = x.Ltp,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionLtpPutWeekly.AddRange(weeklyList);
                        db.OptionLtpPut.RemoveRange(listputltp);
                        db.SaveChanges();
                    }
                    var listcallltq = db.OptionLtqCall.ToList();

                    if (listcallltq.Count > 0)
                    {
                        var weeklyList = listcallltq.Select(x => new OptionLtqCallWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            Ltq = x.Ltq,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionLtqCallWeekly.AddRange(weeklyList);
                        db.OptionLtqCall.RemoveRange(listcallltq);
                        db.SaveChanges();
                    }
                    var listputltq = db.OptionLtqPut.ToList();

                    if (listputltq.Count > 0)
                    {
                        var weeklyList = listputltq.Select(x => new OptionLtqPutWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            Ltq = x.Ltq,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionLtqPutWeekly.AddRange(weeklyList);
                        db.OptionLtqPut.RemoveRange(listputltq);
                        db.SaveChanges();
                    }
                    var listcalloi = db.OptionOICall.ToList();

                    if (listcalloi.Count > 0)
                    {
                        var weeklyList = listcalloi.Select(x => new OptionOICallWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            OI = x.OI,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionOICallWeekly.AddRange(weeklyList);
                        db.OptionOICall.RemoveRange(listcalloi);
                        db.SaveChanges();
                    }
                    var listputoi = db.OptionOIPut.ToList();

                    if (listputoi.Count > 0)
                    {
                        var weeklyList = listputoi.Select(x => new OptionOIPutWeekly
                        {
                            ExchangeInstrumentId = x.ExchangeInstrumentId,
                            OI = x.OI,
                            StrikePrice = x.StrikePrice,
                            Time = x.Time,
                            TimeString = x.TimeString
                        }).ToList();

                        db.OptionOIPutWeekly.AddRange(weeklyList);
                        db.OptionOIPut.RemoveRange(listputoi);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<dynamic> StopFuture()
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
