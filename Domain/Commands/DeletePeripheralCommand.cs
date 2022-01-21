using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for delete a Peripheral device
    /// </summary>
    public class DeletePeripheralCommand : ICommand
    {
        /// <summary>
        /// Peripheral device ID
        /// </summary>
        public int PeripheralId { get; set; }
    }
}
