using System;

namespace Gateways.NET.DTOs
{
    /// <summary>
    /// View model of a Gateway
    /// </summary>
    public class GatewayViewModel
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
    }

    /// <summary>
    /// Full view model of a Gateway, including it's ID
    /// </summary>
    public class FullGatewayViewModel : GatewayViewModel
    {
        /// <summary>
        /// ID of the Gateway
        /// </summary>
        public int Id { get; set; }
    }
}
