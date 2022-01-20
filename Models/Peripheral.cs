using Gateways.NET.Contracts;
using System;

namespace Gateways.NET.Models
{
    /// <summary>
    /// Peripheral device
    /// </summary>
    public class Peripheral : BaseEntity<int>
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

        #region [ Foreign keys ]

        /// <summary>
        /// Related Gateway ID
        /// </summary>
        public int GatewayId { get; set; }

        #endregion

        #region [ Navigation properties ]

        /// <summary>
        /// Related Gateway
        /// </summary>
        public Gateway Gateway { get; set; }

        #endregion
    }
}
