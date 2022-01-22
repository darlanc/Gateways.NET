using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for detach a Peripheral device from it's associated Gateway
    /// </summary>
    public class DetachPeripheralCommand : ICommand
    {
        /// <summary>
        /// ID of the peripheral device
        /// </summary>
        public int PeripheralId { get; set; }
    }
}
