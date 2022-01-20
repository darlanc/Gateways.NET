using Gateways.NET.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateways.NET.Repository.Infraestructure
{
    /// <summary>
    /// Entity Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Db Context</param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region [ Read Operations ]

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, params string[] includeArgs)
        {
            return this.Find(predicate, includeArgs, null);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, string[] includeArgs, params Expression<Func<T, object>>[] navPropertyPath)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            if (navPropertyPath != null && navPropertyPath.Length != 0)
            {
                foreach (var navProperty in navPropertyPath)
                    query = query.Include(navProperty);
            }
            if (includeArgs != null && includeArgs.Length != 0)
            {
                foreach (var navProperty in includeArgs)
                    query = query.Include(navProperty);
            }
            return query;
        }

        public IQueryable<T> FindAll()
        {
            var query = _dbContext.Set<T>();
            return query;
        }

        public async Task<T> FindByIdAsync<Key>(Key id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] includeArgs)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (includeArgs != null && includeArgs.Length != 0)
            {
                foreach (var navProperty in includeArgs)
                    query = query.Include(navProperty);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region [ Write Operations ]
        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Attach(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Remove(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
        }
        public void Update(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
        }
        #endregion
    }
}