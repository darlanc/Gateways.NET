using System.Collections.Generic;
using System.Net;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// Commad response base contract 
    /// </summary>
    public interface ICommandResponse
    {
        /// <summary>
        /// Http code associate with response.
        /// </summary>
        HttpStatusCode Code { get; }

        /// <summary>
        /// Indicate if the answer is satisfactory.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Indicates if should redirect
        /// </summary>
        bool IsRedirect { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> Errors { get; }
    }
}
