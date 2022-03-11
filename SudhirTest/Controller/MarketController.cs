﻿using Binance.Spot;
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
        public async Task<IActionResult> GetMarketDataAsync()
        {
           
            await _marketService.SaveMarketDataAsync();
            return Ok();
        }
        [HttpGet]
        [Route("LoadXTSData")]
        public async Task<IActionResult> LoadXTSData()
        {

            await _marketService.LoadXTSData();
            return Ok();
        }

    }
}
