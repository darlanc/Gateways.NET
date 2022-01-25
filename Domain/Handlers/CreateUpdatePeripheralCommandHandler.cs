using AutoMapper;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.CoreViewModels;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Handlers
{
    public class CreateUpdatePeripheralCommandHandler : ICommandHandler<CreatePeripheralCommand>, ICommandHandler<UpdatePeripheralCommand>
    {
        private readonly IRepository<Peripheral> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUpdatePeripheralCommandHandler(IRepository<Peripheral> repository,
                                                    IUnitOfWork unitOfWork,
                                                    IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICommandResponse> HandleAsync(CreatePeripheralCommand command)
        {
            var item = _mapper.Map<Peripheral>(command);
            item.CreationDate = System.DateTime.Now;
            await _repository.InsertAsync(item);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<FullPeripheralViewModel>(item);
            return command.OkResponse(result);
        }

        public async Task<ICommandResponse> HandleAsync(UpdatePeripheralCommand command)
        {
            var item = await _repository.FindByIdAsync(command.Id);
            if (item == null || item.IsDeleted)
                return command.NotFoundResponse(Resources.ValidationError_PeripheralNotFound);

            //Try to find anoter Peripheral with the same UID, if exists, return validation error
            var other = await _repository.FindFirstOrDefaultAsync(x => x.Id != command.Id && x.UID == command.UID);
            if (other != null)
                return command.BadResponse(Resources.ValidationError_PeripheralUIDAlreadyExist);

            Update(command, item);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<FullPeripheralViewModel>(item);
            return command.OkResponse(result);
        }

        protected virtual void Update(UpdatePeripheralCommand source, Peripheral target)
        {
            target.UID = source.UID;
            target.Vendor = source.Vendor;
            target.Status = source.Status;            
        }
    }
}
