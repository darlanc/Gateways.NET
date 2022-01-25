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
    }
}
