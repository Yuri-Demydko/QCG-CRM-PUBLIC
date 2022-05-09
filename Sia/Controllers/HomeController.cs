using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.ServiceCommon.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly SiaApiClient siad;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SiaApiClient siad)
        {
            _logger = logger;
            this.siad = siad;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Consensus()
        {
            return StatusCode(StatusCodes.Status200OK, await siad.GetConsensusAsync());
        }
        
    }
}