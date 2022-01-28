using Gateways.NET.Contracts;
using Gateways.NET.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Net;

namespace Gateways.NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SystemController : ApiBaseController
    {
        private readonly ILogger<SystemController> _logger;
        private readonly IGatewaysQueryService _queryService;

        public SystemController(ILogger<SystemController> logger, IGatewaysQueryService queryService)
        {           
            _logger = logger;
            _queryService = queryService;
        }

        /// <summary>
        /// Check DB availability.
        /// </summary>
        /// <returns>204 on success, 503 otherwise</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ApiResponse> CheckSystemStatus()
        {
            try
            {
                var source = await _queryService.GetAll(new Pagination { Page = 1, PageSize = 1 });                
                return Respond((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error(Resources.Error_General, StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
