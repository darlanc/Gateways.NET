using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for delete a Gateway
    /// </summary>
    public class DeleteGatewayCommand : IEntityUpdateCommand<int>
    {
        /// <summary>
        /// ID of the Gateway
        /// </summary>
        public int Id { get; set; }
    }
}
