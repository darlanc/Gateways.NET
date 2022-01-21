using System.Collections.Generic;

namespace Gateways.NET.Core
{
    /// <summary>
    /// Paginated List
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Return a Paginated Response
        /// </summary>
        /// <param name="items"></param>
        /// <param name="total"></param>
        /// <param name="pageSize"></param>
        /// <param name="current"></param>
        public PaginatedList(IEnumerable<T> items, int total, int pageSize = 50, int current = 1) : base(items)
        {
            Total = total;
            var totalPages = total % pageSize > 0 ? total / pageSize + 1 : total / pageSize;

            Paging = new Pagination
            {
                Page = current,
                PageSize = pageSize,
                TotalPages = totalPages,
            };            
        }

        /// <summary>
        /// Total
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Paging
        /// </summary>
        public Pagination Paging { get; }
    }
}
