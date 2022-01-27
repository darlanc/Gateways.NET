using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;

namespace Gateways.NET.Domain.Validators
{
    public abstract class CreateUpdatePeripheralValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : PeripheralCommandBase
    {
        public CreateUpdatePeripheralValidator(IRepository<Peripheral> repository)
        {
            RuleFor(b => b.UID)
                .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);            
        }
    }

    public class CreatePeripheralValidator : CreateUpdatePeripheralValidator<CreatePeripheralCommand>
    {
        public CreatePeripheralValidator(IRepository<Peripheral> repository)
            : base(repository)
        {
            RuleFor(p => p).IsUnique(repository).WithMessage(Resources.ValidationError_PeripheralUIDAlreadyExist);
        }
    }

    public class UpdatePeripheralValidator : CreateUpdatePeripheralValidator<UpdatePeripheralCommand>
    {
        public UpdatePeripheralValidator(IRepository<Peripheral> repository)
            : base(repository)
        {
            RuleFor(p => p).ExistsInDatabase(repository).WithMessage(Resources.ValidationError_PeripheralNotFound);
        }
    }

    public class AddPeripheralToGatewayValidator : CreateUpdatePeripheralValidator<AddPeripheralToGatewayCommand>
    {
        public AddPeripheralToGatewayValidator(IRepository<Peripheral> peripheralsRepository, IRepository<Gateway> gatewaysRepository)
            :base(peripheralsRepository)
        {
            RuleFor(p => p).GatewayExistsInDatabase(gatewaysRepository).WithMessage(Resources.ValidationError_GatewayOrPeripheralNotFound);
        }
    }
}
