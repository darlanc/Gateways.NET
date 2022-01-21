using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.Core;
using Gateways.NET.Domain.Commands;
using Gateways.NET.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewaysController : ApiBaseController
    {
        ILogger<GatewaysController> _logger;
        private readonly IMapper _mapper;
        private readonly ICommandDispatcher _dispatcher;

        public GatewaysController(IMapper mapper, ICommandDispatcher dispatcher, ILogger<GatewaysController> logger)
        {
            _mapper = mapper;
            _dispatcher = dispatcher;
            _logger = logger;
        }

        /// <summary>
        /// Add a new Gateway
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<FullGatewayViewModel>> AddGateway(GatewayViewModel model)
        {
            var command = _mapper.Map<CreateGatewayCommand>(model);            
            var commandResponse = await _dispatcher.DispatchAsync(command);
            if (commandResponse.Errors?.Any() == true)
                return Error<FullGatewayViewModel>(commandResponse.Errors, (int)commandResponse.Code);

            var result = (commandResponse as CommandResponse<FullGatewayViewModel>).Body;

            return Respond<FullGatewayViewModel>(payload: result, status: StatusCodes.Status201Created);
        }
    }
}
