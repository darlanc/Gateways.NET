using Gateways.NET.Contracts;
using System;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Base class for create/update Peripheral devices
    /// </summary>
    public class PeripheralCommandBase : ICommand
    {
        /// <summary>
        /// User ID of the device
        /// </summary>
        public uint UID { get; set; }

        /// <summary>
        /// Vendor name
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Status (Online = true / Offline = false)
        /// </summary>
        public bool Status { get; set; }
    }

    /// <summary>
    /// Command for create a Peripheral device
    /// </summary>
    public class CreatePeripheralCommand : PeripheralCommandBase
    {

    }

    /// <summary>
    /// Command for update a Peripheral device
    /// </summary>
    public class UpdatePeripheralCommand : PeripheralCommandBase, IEntityUpdateCommand<int>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Command for create a Peripheral device directly attached to a Gateway
    /// </summary>
    public class AddPeripheralToGatewayCommand : CreatePeripheralCommand
    {
        public int GatewayId { get; set; }
    }
}
