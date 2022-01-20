using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    public interface IQueyRepository<T>
    {
        Task<T> FindByIdAsync<Key>(Key id);

        IQueryable<T> FindAll();

        IQueryable<T> Find(Expression<Func<T, bool>> predicate, params string[] includeArgs);

        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] includeArgs);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate, string[] includeArgs, params Expression<Func<T, object>>[] navPropertyPath);
    }
}
