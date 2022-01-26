using Gateways.NET.Contracts;
using Gateways.NET.Core.Contracts;
using System.Linq;

namespace Gateways.NET.SDK.Contracts
{
    public abstract class ControllerBase : IBackendFolder
    {
        public abstract string FolderName { get; }

        protected virtual T Respond<T>(ApiResponse<T> apiResponse)
        {
            if (!apiResponse.IsSuccess())
            {
                var error = apiResponse.Errors.FirstOrDefault();
                throw new ApiException(error.Message, apiResponse.Status);
            }
            return apiResponse.Payload;
        }
    }
}
