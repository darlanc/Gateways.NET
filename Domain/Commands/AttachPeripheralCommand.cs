using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for attach a peripheral device to a gateway
    /// </summary>
    public class AttachPeripheralCommand : ICommand
    {
        /// <summary>
        /// ID of the Gateway to attach the Peripheral device
        /// </summary>
        public int GatewayId { get; set; }

        /// <summary>
        /// ID of the peripheral device
        /// </summary>
        public int PeripheralId { get; set; }
    }
}
