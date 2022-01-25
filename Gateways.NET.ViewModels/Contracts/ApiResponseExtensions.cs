using Gateways.NET.Contracts;
using System.Net;

namespace Gateways.NET.Core.Contracts
{
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// Indicates wheter the response is successful
        /// </summary>
        public static bool IsSuccess(this ApiResponse response)
        {
            return (int)HttpStatusCode.OK <= (int)response.Status && (int)response.Status <= (int)HttpStatusCode.Ambiguous;
        }
    }
}
