using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;

namespace Gateways.NET.Domain.Validators
{
    public class DetachPeripheralValidator : AbstractValidator<DetachPeripheralCommand>
    {
        public DetachPeripheralValidator(IRepository<Peripheral> repository)
        {
            RuleFor(b => b.Id).NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);
            RuleFor(p => p).ExistsInDatabase(repository).WithMessage(Resources.ValidationError_PeripheralNotFound);
        }
    }
}
