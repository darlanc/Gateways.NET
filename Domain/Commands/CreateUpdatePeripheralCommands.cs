using Gateways.NET.Contracts;
using System;

namespace Gateways.NET.Domain.Commands
{
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
        /// Creation date of the device
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Status (Online = true / Offline = false)
        /// </summary>
        public bool Status { get; set; }
    }

    /// <summary>
    /// Command for create a Peripheral
    /// </summary>
    public class CreatePeripheralCommand : PeripheralCommandBase
    {

    }

    /// <summary>
    /// Command for update a Gateway
    /// </summary>
    public class UpdatePeripheralCommand : PeripheralCommandBase
    {
        public int Id { get; set; }
    }
}
