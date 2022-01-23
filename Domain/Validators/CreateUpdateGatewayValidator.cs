using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;
using Gateways.NET.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.Validators
{
    public abstract class CreateUpdateGatewayValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : GatewayCommandBase
    {
        protected string Ipv4Regex = "^(?:(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])(\\.(?!$)|$)){4}$";

        public CreateUpdateGatewayValidator(IRepository<Gateway> repository)
        {
            RuleFor(b => b.SerialNumber)
                .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired);
            RuleFor(b => b.IpAddress)
                .NotEmpty().WithMessage(Resources.ValidationError_PropertyRequired)
                .DependentRules( ()=> RuleFor(p=>p.IpAddress).Matches(Ipv4Regex) );
            RuleFor(p => p).IsUnique(repository).WithMessage(Resources.ValidationError_GatewaySerialNumberAlreadyExist);
        }
    }
}
