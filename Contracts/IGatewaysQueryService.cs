using Gateways.NET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    public interface IGatewaysQueryService : IQueryService<Gateway>
    {
        Task<IEnumerable<Peripheral>> GetPeripherals(int id);
        Task<bool> Exists(int id);
        Task<Gateway> FindById(int id);
    }
}
