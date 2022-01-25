using System;


namespace Gateways.NET.Contracts
{
    /// <summary>
    /// Pagination Model
    /// </summary>
    public class Pagination : IPaginationModel
    {
        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; } = 20;        
    }

    public class PaginationViewModel : Pagination
    {
        /// <summary>
        /// Total number of pages of the current page size
        /// </summary>
        public int TotalPages { get; set; }
    }
}
