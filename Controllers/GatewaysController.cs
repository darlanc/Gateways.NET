using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.Core;
using Gateways.NET.Domain.Commands;
using Gateways.NET.DTOs;
using Gateways.NET.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
            try
            {
                var command = _mapper.Map<CreateGatewayCommand>(model);
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error<FullGatewayViewModel>(commandResponse.Errors, (int)commandResponse.Code);

                var result = (commandResponse as CommandResponse<FullGatewayViewModel>).Body;

                return Respond<FullGatewayViewModel>(payload: result, status: StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<FullGatewayViewModel>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a Gateway
        /// </summary>
        /// <param name="id">ID of the Gateway</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> DeleteGateway(int id)
        {
            try
            {
                var command = new DeleteGatewayCommand { GatewayId = id };
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error(commandResponse.Errors, (int)commandResponse.Code);

                return Respond(status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<FullGatewayViewModel>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a Gateway properties
        /// </summary>
        /// <param name="model">Gateway model</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<FullGatewayViewModel>> UpdateGateway([FromBody]GatewayViewModel model, int id)
        {
            try
            {
                var command = _mapper.Map<UpdateGatewayCommand>(model);
                command.Id = id;
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error<FullGatewayViewModel>(commandResponse.Errors, (int)commandResponse.Code);

                var result = (commandResponse as CommandResponse<FullGatewayViewModel>).Body;

                return Respond<FullGatewayViewModel>(payload: result, status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<FullGatewayViewModel>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }        
    }
}
