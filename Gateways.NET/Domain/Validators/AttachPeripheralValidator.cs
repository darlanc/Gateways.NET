using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;


namespace Gateways.NET.Domain.Validators
{
    public class AttachPeripheralValidator : AbstractValidator<AttachPeripheralCommand>
    {
        public AttachPeripheralValidator(IRepository<Peripheral> peripheralsRepository, IRepository<Gateway> gatewaysRepository)
        {
            RuleFor(b => b.PeripheralId)
                .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);
            RuleFor(b => b.GatewayId)
                .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);
            RuleFor(p => p).BothExistsInDatabase(gatewaysRepository, peripheralsRepository).WithMessage(Resources.ValidationError_GatewayOrPeripheralNotFound);
        }
    }
}
