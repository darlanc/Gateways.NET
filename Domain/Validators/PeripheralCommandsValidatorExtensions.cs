using FluentValidation;
using Gateways.NET.Contracts;
using Gateways.NET.Domain.Commands;
using Gateways.NET.Models;

namespace Gateways.NET.Domain.Validators
{
    public static class PeripheralCommandsValidatorExtensions
    {
        public static IRuleBuilderOptions<TCommand, TCommand> IsUnique<TCommand>(
            this IRuleBuilder<TCommand, TCommand> ruleBuilder, 
            IRepository<Peripheral> repository) 
            where TCommand : PeripheralCommandBase
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var item = await repository.FindFirstOrDefaultAsync(
                    p =>
                    p.UID == pmCommand.UID);
                return item == null;
            });
        }

        public static IRuleBuilderOptions<TCommand, TCommand> ExistsInDatabase<TCommand>(
            this IRuleBuilder<TCommand, TCommand> ruleBuilder, 
            IRepository<Peripheral> repository) 
            where TCommand : IEntityUpdateCommand<int>
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var item = await repository.FindByIdAsync(pmCommand.Id);
                return item != null;
            });
        }

        public static IRuleBuilderOptions<TCommand, TCommand> BothExistsInDatabase<TCommand>(
            this IRuleBuilder<TCommand, TCommand> ruleBuilder,
            IRepository<Gateway> gatewayRepository,
            IRepository<Peripheral> peripheralRepository)
            where TCommand : AttachPeripheralCommand
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var gateway = await gatewayRepository.FindByIdAsync(pmCommand.GatewayId);
                var peripheral = await peripheralRepository.FindByIdAsync(pmCommand.PeripheralId);

                return gateway != null && peripheral != null;
            });
        }

        public static IRuleBuilderOptions<TCommand, TCommand> GatewayExistsInDatabase<TCommand>(this IRuleBuilder<TCommand, TCommand> ruleBuilder, IRepository<Gateway> repository) where TCommand : AddPeripheralToGatewayCommand
        {
            return ruleBuilder.MustAsync(async (command, pmCommand, cancellation) =>
            {
                var item = await repository.FindByIdAsync(pmCommand.GatewayId);
                return item != null;
            });
        }
    }
}
