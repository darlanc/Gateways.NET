using Gateways.NET.ViewModels;
using Gateways.NET.CoreViewModels;
using System.Collections.Generic;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// Basic API Response
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// Errors
        /// </summary>
        public IEnumerable<Error> Errors { get; set; }        
    }

    /// <summary>
    /// Generic payload API response
    /// </summary>
    /// <typeparam name="T">Generic payload type</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Payload
        /// </summary>
        public T Payload { get; set; }
    }

    /// <summary>
    /// Generic paginated payload API response
    /// </summary>
    /// <typeparam name="T">Generic payload type</typeparam>
    public class PaginatedApiResponse<T> : ApiResponse<T>
    {
        /// <summary>
        /// Paging information
        /// </summary>
        public Pagination Paging { get; set; }
    }
}
