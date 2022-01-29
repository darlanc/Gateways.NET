using Gateways.NET.Models;
using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    public interface IPeripheralsQueryService : IQueryService<Peripheral>
    {
        Task<bool> Exists(int id);
        Task<Peripheral> FindById(int id);
    }
}
