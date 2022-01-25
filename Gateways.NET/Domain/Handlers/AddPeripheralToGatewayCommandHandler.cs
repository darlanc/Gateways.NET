using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.ViewModels;
using Gateways.NET.Domain.Commands;
using Gateways.NET.CoreViewModels;
using Gateways.NET.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class AddPeripheralToGatewayCommandHandler : ICommandHandler<AddPeripheralToGatewayCommand>        
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IGatewaysQueryService _gatewaysQueryService;
        private readonly IMapper _mapper;

        public AddPeripheralToGatewayCommandHandler(ICommandDispatcher dispatcher, 
                                                    IGatewaysQueryService gatewaysQueryService,
                                                    IMapper mapper)
        {
            _dispatcher = dispatcher;
            _gatewaysQueryService = gatewaysQueryService;
            _mapper = mapper;
        }

        public async Task<ICommandResponse> HandleAsync(AddPeripheralToGatewayCommand command)
        {
            if (!await _gatewaysQueryService.Exists(command.GatewayId))
                return command.NotFoundResponse(Resources.ValidationError_GatewayNotFound);
            
            var createPeripheralCommand = _mapper.Map<CreatePeripheralCommand>(command);
            var createResponse = await _dispatcher.DispatchAsync(createPeripheralCommand);
            if (!createResponse.Success)
                return command.ErrorResponse(createResponse.Errors.FirstOrDefault(), createResponse.Code);

            var peripheral = (createResponse as CommandResponse<FullPeripheralViewModel>).Body;

            var attachPeripheralCommand = new AttachPeripheralCommand { GatewayId = command.GatewayId, PeripheralId = peripheral.Id };
            var attachResponse = await _dispatcher.DispatchAsync(attachPeripheralCommand);
            if (!attachResponse.Success)
                return command.ErrorResponse(attachResponse.Errors.FirstOrDefault(), attachResponse.Code);
            peripheral.GatewayId = command.GatewayId;

            var result = peripheral;

            return command.OkResponse(peripheral);
        }
    }
}
