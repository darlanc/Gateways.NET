using Gateways.NET.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gateways.NET.Repository.Infraestructure
{
    /// <summary>
    /// Base class of a Unit of Work implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="context">Associated DbContext</param>
        public UnitOfWork(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Saves changes to the underlying context
        /// </summary>
        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves changes to the underlying context
        /// </summary>
        /// <returns></returns>
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        #region [ IDisposable Support ]

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Implementation of the Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);            
        }

        #endregion
    }
}
