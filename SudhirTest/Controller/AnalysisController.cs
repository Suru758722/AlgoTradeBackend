using Microsoft.AspNetCore.Mvc;
using SudhirTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudhirTest.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }
        [HttpPost]
        [Route("LoadBhavCopy")]
        public IActionResult LoadBhavCopy(object data)
        {
            _analysisService.LoadBhavCopy(data);
            return Ok();
        }
        [HttpGet]
        [Route("GetInstrument")]
        public IActionResult GetInstrument()
        {            
            return Ok(_analysisService.GetInstrument());
        }
    }
}
