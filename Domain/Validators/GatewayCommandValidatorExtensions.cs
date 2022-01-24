using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;

namespace Gateways.NET.Domain.Validators
{
    public static class GatewayCommandValidatorExtensions
    {
        public static IRuleBuilderOptions<TCommand, TCommand> IsUnique<TCommand>(this IRuleBuilder<TCommand, TCommand> ruleBuilder, IRepository<Gateway> repository) where TCommand : GatewayCommandBase
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var item = await repository.FindFirstOrDefaultAsync(
                    p =>
                    p.SerialNumber == pmCommand.SerialNumber);
                return item == null;
            });
        }

        public static IRuleBuilderOptions<TCommand, TCommand> ExistsInDatabase<TCommand>(this IRuleBuilder<TCommand, TCommand> ruleBuilder, IRepository<Gateway> repository) where TCommand : IEntityUpdateCommand<int>
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var item = await repository.FindByIdAsync(pmCommand.Id);
                return item != null;
            });
        }        
    }
}
