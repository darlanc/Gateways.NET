using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for delete a Gateway
    /// </summary>
    public class DeleteGatewayCommand : ICommand
    {
        /// <summary>
        /// ID of the Gateway
        /// </summary>
        public int GatewayId { get; set; }
    }
}
