using Microsoft.AspNetCore.Mvc;
using SudhirTest.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudhirTest.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly IOptionService _optionService;
        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }


        [HttpGet]
        [Route("SaveOptionData")]
        public async Task<bool> SaveOptionData()
        {

          return  await _optionService.SaveOptionData();
        }
        [HttpGet]
        [Route("StopOption")]
        public async Task<bool> StopOption()
        {

         return  await _optionService.StopOption();
        }
    }

}
