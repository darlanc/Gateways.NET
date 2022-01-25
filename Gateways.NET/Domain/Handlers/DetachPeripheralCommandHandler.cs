using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class DetachPeripheralCommandHandler : ICommandHandler<DetachPeripheralCommand>
    {
        private readonly IRepository<Peripheral> _repository;
        private readonly IUnitOfWork _unitOfWork;        

        public DetachPeripheralCommandHandler(IRepository<Peripheral> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ICommandResponse> HandleAsync(DetachPeripheralCommand command)
        {
            var item = await _repository.FindByIdAsync(command.Id);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_PeripheralNotFound);
                        
            item.GatewayId = null;
            await _unitOfWork.SaveChangesAsync();

            return command.OkResponse(true);
        }
    }
}
