using System;
using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// General unit of work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves changes to the underlying context
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Asynchronously saves changes to the underlying context
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
