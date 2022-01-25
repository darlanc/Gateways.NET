using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// General command dispatcher contract
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatches a command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        Task<ICommandResponse> DispatchAsync(ICommand command);
    }
}
