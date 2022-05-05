using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SudhirTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IIndexService _indexService;
        public IndexController(IIndexService indexService)
        {
            _indexService = indexService;
        }


        [HttpGet]
        [Route("SaveOptionInstrument")]
        public async Task<bool> SaveOptionInstrument()
        {

           return await _indexService.SaveOptionInstrument();
           
        }
        [HttpGet]
        [Route("SaveFutureInstrument")]
        public async Task<bool> SaveFutureInstrument()
        {

          return  await _indexService.SaveFutureInstrument();
            
        }

        [HttpGet]
        [Route("SaveFutureData")]
        public async Task<bool> SaveFutureData()
        {

           return await _indexService.SaveFutureData();
            
        }

        [HttpGet]
        [Route("DeletePreviousData")]
        public bool DeletePreviousData()
        {

            return _indexService.DeletePreviousData();
            
        }
        [HttpGet]
        [Route("StopFuture")]
        public async Task<bool> StopFuture()
        {

           return await _indexService.StopFuture();
           
        }
    }
}
