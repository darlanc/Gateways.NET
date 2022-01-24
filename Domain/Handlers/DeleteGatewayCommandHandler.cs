using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class DeleteGatewayCommandHandler : ICommandHandler<DeleteGatewayCommand>
    {
        private readonly IRepository<Gateway> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGatewayCommandHandler(IRepository<Gateway> repository,
                                           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> HandleAsync(DeleteGatewayCommand command)
        {
            var item = await _repository.FindByIdAsync(command.Id);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_GatewayNotFound);

            // Soft delete behavior
            item.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return command.OkResponse(true);
        }
    }
}
