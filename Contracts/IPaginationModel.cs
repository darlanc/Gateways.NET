using System;


namespace Gateways.NET.Contracts
{
    /// <summary>
    /// General pagination model interface
    /// </summary>
    public interface IPaginationModel
    {
        /// <summary>
        /// Page index
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Total number of pages of the current page size
        /// </summary>
        int TotalPages { get; set; }
    }
}
