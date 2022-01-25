using Gateways.NET.Contracts;
using Gateways.NET.Models;

namespace Gateways.NET.Domain.Commands
{
    public class GatewayCommandBase : ICommand
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
    /// Command for create a Gateway
    /// </summary>
    public class CreateGatewayCommand : GatewayCommandBase
    {

    }

    /// <summary>
    /// Command for update a Gateway
    /// </summary>
    public class UpdateGatewayCommand : GatewayCommandBase, IEntityUpdateCommand<int>
    {
        /// <summary>
        /// ID of the Gateway
        /// </summary>
        public int Id { get; set; }
    }
}
