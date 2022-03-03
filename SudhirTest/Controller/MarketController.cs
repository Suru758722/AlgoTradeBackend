using Binance.Spot;
using DotNetify;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SudhirTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudhirTest.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
       // readonly ApplicationDbContext _dbcontext;
        //private IHubContext<DotNetifyHub> _hub;
        //private readonly HttpClient _http; 

        public MarketController()
        {
            //_dbcontext = dbcontext;
           // _http = http;

        }

        [HttpGet]
        [Route("GetMarketData")]
        public dynamic GetMarketDataAsync()
        {
            var timer = Observable.Interval(TimeSpan.FromSeconds(5));
            timer.Subscribe(async x =>
            {
                x += 31;
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri("https://api.binance.com");
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("https://api.binance.com/api/v3/aggTrades?symbol=BNBBTC");
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                       var EmpInfo = JsonConvert.DeserializeObject(EmpResponse);
                        var temp = EmpInfo;
                    }
                    //returning the employee list to view
                   
                }

            });
            //var websocket = new MarketDataWebSocket("btcusdt@aggTrade");

            //websocket.OnMessageReceived(
            //    (data) =>
            //    {
            //        var temp = data;

            //        return Task.CompletedTask;
            //    }, CancellationToken.None);

            //await websocket.ConnectAsync(CancellationToken.None);
            return true;
        }

    }
}
