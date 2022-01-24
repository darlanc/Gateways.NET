using Gateways.NET.Contracts;

namespace Gateways.NET.Domain.Commands
{
    /// <summary>
    /// Command for delete a Peripheral device
    /// </summary>
    public class DeletePeripheralCommand : IEntityUpdateCommand<int>
    {
        /// <summary>
        /// Peripheral device ID
        /// </summary>
        public int Id { get; set; }
    }
}
