using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for set a Peripheral device status ON/OFF
    /// </summary>
    public class SetPeripheralStatusCommand : ICommand
    {
        /// <summary>
        /// Peripheral device ID
        /// </summary>
        public int PeripheralId { get; set; }

        /// <summary>
        /// Status of the Peripheral device
        /// </summary>
        public bool Status { get; set; }
    }
}
