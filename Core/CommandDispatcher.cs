using FluentValidation;
using Gateways.NET.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _provider;

        public CommandDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Dispatches the command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        public async Task<ICommandResponse> DispatchAsync(ICommand command)
        {
            using (var scope = this._provider.CreateScope())
            {
                Type commandType = command.GetType();

                // getting and executing asosiated validator.
                var validatorType = typeof(IValidator<>).MakeGenericType(commandType);
                if (scope.ServiceProvider.GetService(validatorType) is IValidator validator)
                {
                    var errors = await validator.ValidateAsync(command);
                    if (errors?.IsValid == false)
                    {
                        return command.BadResponse(errors.Errors.Select(e => e.ErrorMessage));
                    }
                }

                ICommandHandler handler = GetHandler(scope.ServiceProvider, commandType);

                return await ((dynamic)handler).HandleAsync((dynamic)command);
            }
        }

        private static ICommandHandler GetHandler(IServiceProvider scopeProvider, Type command)
        {
            Type serviceType = typeof(ICommandHandler<>).MakeGenericType(command);
            ICommandHandler commandHandler = (ICommandHandler)scopeProvider.GetService(serviceType);
            if (commandHandler == null)
                throw new KeyNotFoundException("There is no handler associated with this command");

            return commandHandler;
        }
    }
}
