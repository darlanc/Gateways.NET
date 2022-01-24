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
    public class PeripheralsController : ApiBaseController
    {
        ILogger<PeripheralsController> _logger;
        private readonly IMapper _mapper;
        private readonly ICommandDispatcher _dispatcher;

        public PeripheralsController(IMapper mapper, ICommandDispatcher dispatcher, ILogger<PeripheralsController> logger)
        {
            _mapper = mapper;
            _dispatcher = dispatcher;
            _logger = logger;
        }

        /// <summary>
        /// Add a new Peripheral device
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<FullPeripheralViewModel>> AddPeripheral(GatewayViewModel model)
        {
            try
            {
                var command = _mapper.Map<CreatePeripheralCommand>(model);
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error<FullPeripheralViewModel>(commandResponse.Errors, (int)commandResponse.Code);

                var result = (commandResponse as CommandResponse<FullPeripheralViewModel>).Body;

                return Respond<FullPeripheralViewModel>(payload: result, status: StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<FullPeripheralViewModel>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a Peripheral device
        /// </summary>
        /// <param name="id">ID of the Peripheral device</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> DeletePeripheral(int id)
        {
            try
            {
                var command = new DeletePeripheralCommand { Id = id };
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error(commandResponse.Errors, (int)commandResponse.Code);

                return Respond(status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a Peripheral device properties
        /// </summary>
        /// <param name="model">Peripheral device model</param>
        /// <param name="id">ID of the Peripheral device</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<FullPeripheralViewModel>> UpdatePeripheral([FromBody] PeripheralViewModel model, int id)
        {
            try
            {
                var command = _mapper.Map<UpdatePeripheralCommand>(model);
                command.Id = id;
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error<FullPeripheralViewModel>(commandResponse.Errors, (int)commandResponse.Code);

                var result = (commandResponse as CommandResponse<FullPeripheralViewModel>).Body;

                return Respond<FullPeripheralViewModel>(payload: result, status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<FullPeripheralViewModel>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Attaches a Peripheral device to a Gateway
        /// </summary>
        /// <param name="model">Peripheral device Attach model</param>
        /// /// <param name="id">ID of the Peripheral device</param>
        /// <returns></returns>
        [HttpPatch("{id}/attach")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> AttachPeripheral([FromBody]AttachPeripheralViewModel model, int id)
        {
            try
            {
                var command = new AttachPeripheralCommand
                {
                    PeripheralId = id,
                    GatewayId = model.GatewayId
                };

                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error(commandResponse.Errors, (int)commandResponse.Code);

                return Respond(status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Detaches a Peripheral device from it's currently associated Gateway
        /// </summary>
        /// <param name="id">ID of the Peripheral device</param>
        /// <returns></returns>
        [HttpPatch("{id}/detach")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> DetachPeripheral(int id)
        {
            try
            {
                var command = new DetachPeripheralCommand
                {
                    Id = id                    
                };

                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error(commandResponse.Errors, (int)commandResponse.Code);

                return Respond(status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a Peripheral device status (ON/OFF)
        /// </summary>
        /// <param name="model">Peripheral device model</param>
        /// /// <param name="id">ID of the Peripheral device</param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> UpdatePeripheralStatus([FromBody]PeripheralStatusViewModel model, int id)
        {
            try
            {
                var command = new SetPeripheralStatusCommand
                {
                    PeripheralId = id,
                    Status = model.Status
                };
                
                var commandResponse = await _dispatcher.DispatchAsync(command);
                if (commandResponse.Errors?.Any() == true)
                    return Error(commandResponse.Errors, (int)commandResponse.Code);                

                return Respond(status: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
