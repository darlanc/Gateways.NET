using System;

namespace Gateways.NET.DTOs
{
    /// <summary>
    /// View model of a Peripheral device
    /// </summary>
    public class PeripheralViewModel
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
    /// Full view model of a Peripheral device, including it's ID and the ID of the associated Gateway (if there is one)
    /// </summary>
    public class FullPeripheralViewModel : PeripheralViewModel
    {
        /// <summary>
        /// ID of the Peripheral device
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the associated Gateway (if there is one)
        /// </summary>
        public int? GatewayId { get; set; }
    }
}
