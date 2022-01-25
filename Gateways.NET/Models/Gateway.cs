using Gateways.NET.Contracts;
using System;
using System.Collections.Generic;

namespace Gateways.NET.Models
{
    public class Gateway : BaseEntity<int>
    {
        /// <summary>
        /// Serial number of the Gateway (unique)
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Name of the Gateway
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IPv4 address of the Gateway
        /// </summary>
        public string IpAddress { get; set; }

        #region [ Navigation Properties ]

        /// <summary>
        /// Associated peripheral devices
        /// </summary>
        public ICollection<Peripheral> Peripherals { get; set; }

        #endregion
    }
}
