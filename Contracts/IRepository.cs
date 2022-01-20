using System.Threading.Tasks;

namespace Gateways.NET.Contracts
{
    public interface IRepository<T> : IQueyRepository<T>
    {
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Attach(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
