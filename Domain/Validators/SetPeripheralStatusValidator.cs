using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;

namespace Gateways.NET.Domain.Validators
{
    public class SetPeripheralStatusValidator : AbstractValidator<SetPeripheralStatusCommand>
    {
        public SetPeripheralStatusValidator(IRepository<Peripheral> repository)
        {
            RuleFor(b => b.PeripheralId)
               .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);
        }
    }
}
