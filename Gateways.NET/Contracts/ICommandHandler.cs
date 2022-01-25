using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// General command handler contract
    /// </summary>
    public interface ICommandHandler
    {
    }

    /// <summary>
    /// Generic command handler contract
    /// </summary>
    /// <typeparam name="TCommand">Command generic type</typeparam>
    public interface ICommandHandler<TCommand> : ICommandHandler where TCommand : ICommand
    {
        /// <summary>
        /// Handles the given command
        /// </summary>
        /// <param name="command">Command with data needed to handle operation.</param>
        /// <returns></returns>
        Task<ICommandResponse> HandleAsync(TCommand command);
    }
}
