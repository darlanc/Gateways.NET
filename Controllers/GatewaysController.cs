using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewaysController : ControllerBase
    {
        ILogger<GatewaysController> _logger;
        public GatewaysController(ILogger<GatewaysController> logger)
        {
            _logger = logger;
        }
    }
}
