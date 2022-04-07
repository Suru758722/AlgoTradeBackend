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
        public async Task<IActionResult> SaveOptionInstrument()
        {

            await _indexService.SaveOptionInstrument();
            return Ok();
        }
    }
}
