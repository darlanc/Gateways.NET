using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class DeletePeripheralComandHandler : ICommandHandler<DeletePeripheralCommand>
    {
        private readonly IRepository<Peripheral> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandDispatcher _dispatcher;

        public DeletePeripheralComandHandler(IRepository<Peripheral> repository, 
                                             IUnitOfWork unitOfWork, 
                                             ICommandDispatcher dispatcher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _dispatcher = dispatcher;
        }

        public async Task<ICommandResponse> HandleAsync(DeletePeripheralCommand command)
        {
            var item = await _repository.FindByIdAsync(command.Id);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_PeripheralNotFound);

            // Detaches the Peripheral first if attached to a Gateway
            if (item.GatewayId != null)
            {
                var detachCommand = new DetachPeripheralCommand
                {
                    Id = command.Id
                };
                var detachResponse = await _dispatcher.DispatchAsync(detachCommand);
                if (!detachResponse.Success)
                    return command.ErrorResponse(detachResponse.Errors.FirstOrDefault(), detachResponse.Code);
            }

            // Soft delete behavior
            item.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return command.OkResponse(true);
        }
    }
}
