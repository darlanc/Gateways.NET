using Gateways.NET.Core;
using Gateways.NET.DTOs;
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
        /// <summary>
        /// Paging
        /// </summary>
        public Pagination Paging { get; set; }
    }

    /// <summary>
    /// Generic Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Payload
        /// </summary>
        public T Payload { get; set; }
    }
}
