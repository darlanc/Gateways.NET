using Gateways.NET.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateways.NET.Core
{
    /// <summary>
    /// Generic base class of query services over entities
    /// </summary>
    /// <typeparam name="TEntity">Generic entity type</typeparam>
    public abstract class BaseQueryService<TEntity> where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;

        protected BaseQueryService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Search with pagination and ordering support
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="orders"></param>
        /// <param name="includes"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        protected async Task<(IEnumerable<TEntity>, int)> PaginationSearch(
            PaginationViewModel pagination,
            IEnumerable<OrderColumn> orders,
            string[] includes,
            params Expression<Func<TEntity, bool>>[] criterias)
        {
            var query = Search(orders, includes, criterias);

            var total = query.Count();

            if (pagination != null)
                query = QueryHelper.ApplyPagging(query, pagination.Page, pagination.PageSize);

            return await query.ToListAsync().ContinueWith(c => {
                return (c.Result, total);
            });
        }

        /// <summary>
        /// Search and ordering support
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="includes"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> Search(
            IEnumerable<OrderColumn> orders,
            string[] includes,
            params Expression<Func<TEntity, bool>>[] criterias)
        {
            IQueryable<TEntity> query = _repository.Find(null, includes, null);

            query = QueryHelper.ApplyFilters(query, criterias);

            query = QueryHelper.ApplyOrder(query, orders);

            return query;
        }
    }
}
