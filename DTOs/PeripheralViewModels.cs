using System;

namespace Gateways.NET.DTOs
{
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
}
