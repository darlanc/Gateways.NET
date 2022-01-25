using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// Basic query service interface
    /// </summary>
    public interface IQueryService
    {
    }

    /// <summary>
    /// Generic query service interface
    /// </summary>
    /// <typeparam name="T">Generic entity type</typeparam>
    public interface IQueryService<T> : IQueryService
    {
        Task<IEnumerable<T>> GetAll(IPaginationModel pagination);
    }
}
