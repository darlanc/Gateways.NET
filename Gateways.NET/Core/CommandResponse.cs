using Gateways.NET.Contracts;
using System.Collections.Generic;
using System.Net;

namespace Gateways.NET.ViewModels
{
    /// <summary>
    /// Basic command response implementation
    /// </summary>
    /// <typeparam name="TBody">Generic type of the response body</typeparam>
    public class CommandResponse<TBody> : ICommandResponse
    {
        public CommandResponse(HttpStatusCode code, TBody body, IEnumerable<string> errors = null)
        {
            this.Code = code;
            this.Body = body;
            this.Errors = errors;
        }

        /// <summary>
        /// Generic response body
        /// </summary>
        public TBody Body { get; }

        /// <summary>
        /// Http code associate with response.
        /// </summary>
        public HttpStatusCode Code { get; }

        /// <summary>
        /// Indicates wheter the response is successful
        /// </summary>
        public bool Success => (int)HttpStatusCode.OK <= (int)Code && (int)Code <= (int)HttpStatusCode.Ambiguous;

        /// <summary>
        /// Indicates wheter the response is a redirect
        /// </summary>
        public bool IsRedirect => (int)HttpStatusCode.Ambiguous <= (int)Code && (int)Code <= (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Error messages
        /// </summary>
        public IEnumerable<string> Errors { get; }
    }
}
