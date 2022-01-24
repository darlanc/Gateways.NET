using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class AttachPeripheralCommandHandler : ICommandHandler<AttachPeripheralCommand>
    {
        private readonly IRepository<Peripheral> _repository;
        private readonly IGatewaysQueryService _gatewaysQueryService;
        private readonly IUnitOfWork _unitOfWork;        

        protected static int MaxPeripherals => 10;

        public AttachPeripheralCommandHandler(IRepository<Peripheral> repository,
                                              IGatewaysQueryService gatewaysQueryService,
                                              IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _gatewaysQueryService = gatewaysQueryService;
            _unitOfWork = unitOfWork;            
        }

        public async Task<ICommandResponse> HandleAsync(AttachPeripheralCommand command)
        {
            var item = await _repository.FindByIdAsync(command.PeripheralId);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_PeripheralNotFound);

            var gateway = await _gatewaysQueryService.FindById(command.GatewayId);
            if (gateway == null)
                return command.NotFoundResponse(Resources.ValidationError_GatewayNotFound);

            if (gateway.Peripherals.Count >= MaxPeripherals)
                return command.BadResponse(string.Format(Resources.Error_FullGateway, MaxPeripherals));

            item.GatewayId = command.GatewayId;
            await _unitOfWork.SaveChangesAsync();
            return command.OkResponse(true);
        }
    }
}
