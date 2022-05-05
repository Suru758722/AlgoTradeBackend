using Binance.Spot;
using DotNetify;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SudhirTest.Data;
using SudhirTest.Services;
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
        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;

        }

        [HttpGet]
        [Route("GetMarketData")]
        public async Task<bool> GetMarketData()
        {
           
            return await _marketService.SaveMarketDataAsync();
        }
        [HttpGet]
        [Route("TestMethod")]
        public async Task<IActionResult> TestMethod()
        {

            await _marketService.TestMethod();
            return Ok();
        }

        [HttpGet]
        [Route("StopStock")]
        public async Task<bool> StopStock()
        {

            return await _marketService.StopStock();
        }
    }
}
