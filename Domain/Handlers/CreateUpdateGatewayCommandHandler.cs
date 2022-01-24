using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.DTOs;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class CreateUpdateGatewayCommandHandler : ICommandHandler<CreateGatewayCommand>, ICommandHandler<UpdateGatewayCommand>
    {
        private readonly IRepository<Gateway> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUpdateGatewayCommandHandler(IRepository<Gateway> repository,
                                                 IUnitOfWork unitOfWork,
                                                 IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICommandResponse> HandleAsync(CreateGatewayCommand command)
        {
            var item = _mapper.Map<Gateway>(command);
            await _repository.InsertAsync(item);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<FullGatewayViewModel>(item);
            return command.OkResponse(result);
        }

        public async Task<ICommandResponse> HandleAsync(UpdateGatewayCommand command)
        {
            var item = await _repository.FindByIdAsync(command.Id);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_GatewayNotFound);

            //Try to find anoter Gateway with the same Serial Number, if exists, return validation error
            var other = await _repository.FindFirstOrDefaultAsync(x => x.Id != command.Id && x.SerialNumber == command.SerialNumber);
            if (other != null)
                return command.BadResponse(Resources.ValidationError_GatewaySerialNumberAlreadyExist);

            Update(command, item);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<FullGatewayViewModel>(item);
            return command.OkResponse(result);
        }

        protected virtual void Update(UpdateGatewayCommand source, Gateway target)
        {
            target.SerialNumber = source.SerialNumber;
            target.IpAddress = source.IpAddress;
            target.Name = source.Name;
        }
    }
}
