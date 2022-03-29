﻿using Microsoft.AspNetCore.Mvc;
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
    public class OptionController : ControllerBase
    {
        private readonly IOptionService _optionService;
        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }


        [HttpGet]
        [Route("SaveOptionData")]
        public async Task<IActionResult> SaveOptionData()
        {

            await _optionService.SaveOptionData();
            return Ok();
        }
    }
}