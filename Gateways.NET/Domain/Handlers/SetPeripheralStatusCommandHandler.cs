using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class SetPeripheralStatusCommandHandler : ICommandHandler<SetPeripheralStatusCommand>
    {
        private readonly IRepository<Peripheral> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SetPeripheralStatusCommandHandler(IRepository<Peripheral> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> HandleAsync(SetPeripheralStatusCommand command)
        {
            var item = await _repository.FindByIdAsync(command.PeripheralId);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_PeripheralNotFound);

            item.Status = command.Status;
            await _unitOfWork.SaveChangesAsync();

            return command.OkResponse(true);
        }
    }
}
