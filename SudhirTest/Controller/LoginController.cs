using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SocketIOClient;
using SudhirTest.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudhirTest.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        static HttpClient _httpClient { get; set; } = null;
        public MarketDataPorts MarketDataPorts { get; set; } = MarketDataPorts.marketDepthEvent;


        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        // GET: api/<LoginController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            const string URL = "https://xts.compositedge.com";

            const string MARKET_APPKEY = "e16c965046e51516c3e171";
            const string MARKET_SECRET = "Hbui010#Oq";
            string USER_ID = "JHS04";
            if (Uri.TryCreate(URL, UriKind.Absolute, out Uri result))
            {
                string authority = result.GetLeftPart(UriPartial.Authority);

                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(authority);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }

            var payload = new
            {
                appKey = MARKET_APPKEY,
                secretKey = MARKET_SECRET,
                source = "WEBAPI"
            };
            var response = await _httpClient.PostAsync("/marketdata/auth/login", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")).ConfigureAwait(false);

            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(str);
            _httpClient.DefaultRequestHeaders.Add("authorization", loginResponse.Result.token);

            var socket = new SocketIO(URL, new SocketIOOptions
            {
                EIO = 3,
                Path = "/marketdata/socket.io",
                Query = new Dictionary<string, string>()
                    {
                        { "token", loginResponse.Result.token },
                        { "userID", USER_ID },
                        { "source", "WebAPI" },
                        { "publishFormat", "JSON" },
                        { "broadcastMode", "Partial" }
                    }
            });
            socket.On("1502-json-partial", response =>
            {
                //ts = DateTime.Now - lastDate;
                var obj = response.GetValue();
                // Console.WriteLine(obj.ToString());
            });
            socket.On("1502-json-full", response =>
            {
                //store data in rethinkdb

                var obj = response.GetValue();

                var mdp = JsonConvert.DeserializeObject<MdpModel>(obj.ToString(), new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });
                //using (var context = _contextFactory.CreateDbContext())
                //{
                //    SymbolData symbol = new SymbolData();
                //    symbol.SymbolId = 1;
                //    symbol.Price = 546;
                //    symbol.Time = DateTime.Now;
                //    context.SymbolData.Add(symbol);
                //    context.SaveChanges();
                //}
                //mdp.SymbolName = "CRUDEOIL 19AUG2021";
                ////_rDb.InsertMdp("19AUG2021", "CRUDEOIL_N", mdp);
                //var mdp = JsonConvert.DeserializeObject<MdpModel>(obj);
                var temp = mdp;

            });





            await socket.ConnectAsync();

            await SubscribeAsync();
            return Ok();
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private async Task SubscribeAsync()
        {

            SubscriptionPayload payload = new SubscriptionPayload()
            {
                instruments = GetInstruments(MarketDataPorts),
                xtsMessageCode = 1502
            };

            var response = await _httpClient.PostAsync(@"/marketdata/instruments/subscription", payload?.GetHttpContent()).ConfigureAwait(false);

            string txt;

            if (response != null)
            {
                txt = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Console.WriteLine(txt.ToString());
            }


        }
        private List<Instruments> GetInstruments(MarketDataPorts port)
        {

            //int exchange = (int)ExchangeSegment.MCXFO;
            int exchange = 3;
            long exchangeInstrumentId = 228031;   //reliance

            if (this.MarketDataPorts == MarketDataPorts.openInterestEvent)
            {
                exchange = (int)ExchangeSegment.NSEFO;
                exchangeInstrumentId = 45042; //nifty sep 19 fut
            }
            else if (this.MarketDataPorts == MarketDataPorts.indexDataEvent)
            {
                exchangeInstrumentId = 1;
            }

            return new List<Instruments>()
            {
                new Instruments()
                {
                    exchangeInstrumentID = exchangeInstrumentId,
                    exchangeSegment = exchange
                }
            };

        }
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
