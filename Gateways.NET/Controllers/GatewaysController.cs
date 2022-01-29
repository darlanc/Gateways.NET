using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.ViewModels;
using Gateways.NET.Domain.Commands;
using Gateways.NET.CoreViewModels;
using Gateways.NET.Properties;
using Microsoft.AspNetCore.Http;
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
    public class GatewaysController : ApiBaseController
    {
        private readonly ILogger<GatewaysController> _logger;
        private readonly IMapper _mapper;
        private readonly ICommandDispatcher _dispatcher;
        private readonly IGatewaysQueryService _queryService;

        public GatewaysController(IMapper mapper, ICommandDispatcher dispatcher, ILogger<GatewaysController> logger, IGatewaysQueryService queryService)
        {
            _mapper = mapper;
            _dispatcher = dispatcher;
            _logger = logger;
            _queryService = queryService;
        }

        /// <summary>
        /// Add a new Gateway
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ApiResponse<FullGatewayViewModel>> AddGateway([FromBody]GatewayViewModel model)
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
                var command = new DeleteGatewayCommand { Id = id };
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
        /// Updates a Gateway properties
        /// </summary>
        /// <param name="model">Gateway model</param>
        /// /// <param name="id">Gateway ID</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<FullGatewayViewModel>> UpdateGateway([FromBody] GatewayViewModel model, int id)
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

        /// <summary>
        /// Add a new Peripheral device and attaches it to the Gateway
        /// </summary>
        /// <param name="model">Peripheral device model</param>
        /// /// <param name="id">Gateway ID</param>
        /// <returns></returns>
        [HttpPost("{id}/peripherals")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<FullPeripheralViewModel>> AddPeripheral([FromBody] PeripheralViewModel model, int id)
        {
            try
            {
                var command = _mapper.Map<AddPeripheralToGatewayCommand>(model);
                command.GatewayId = id;
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
        /// Get the list of existing gateways and their associated peripheral devices.
        /// </summary>
        /// <returns>List of Gateways</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse<IEnumerable<FullGatewayViewModel>>> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var source = await _queryService.GetAll(pagination);
                var result = _mapper.Map<IEnumerable<FullGatewayViewModel>>(source);
                return Respond<IEnumerable<FullGatewayViewModel>>(payload: result);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<IEnumerable<FullGatewayViewModel>> (Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the list of associated peripheral devices of a Gateway.
        /// </summary>
        /// <returns>List of Peripherals</returns>
        [HttpGet("{id}/peripherals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse<IEnumerable<FullPeripheralViewModel>>> GetPeripherals(int id)
        {
            try
            {
                var exists = await _queryService.Exists(id);
                if (!exists)
                    return Error<IEnumerable<FullPeripheralViewModel>>(Resources.ValidationError_GatewayNotFound, StatusCodes.Status404NotFound);
                var source = await _queryService.GetPeripherals(id);
                var result = _mapper.Map<IEnumerable<FullPeripheralViewModel>>(source);
                return Respond<IEnumerable<FullPeripheralViewModel>>(payload: result);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, ex.Message);
                return Error<IEnumerable<FullPeripheralViewModel>>(Resources.Error_General, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get a Gateway properties and the list of associated peripheral devices.
        /// </summary>
        /// <returns>A Gateway model</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse<FullGatewayViewModel>> GetGateway(int id)
        {
            try
            {
                var gateway = await _queryService.FindById(id);
                if (gateway == null)
                    return Error<FullGatewayViewModel>(Resources.ValidationError_GatewayNotFound, StatusCodes.Status404NotFound);
                var result = _mapper.Map<FullGatewayViewModel>(gateway);
                return Respond<FullGatewayViewModel>(payload: result);
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
