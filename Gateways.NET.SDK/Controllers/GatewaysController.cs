using Gateways.NET.Contracts;
using Gateways.NET.Core.Contracts;
using Gateways.NET.CoreViewModels;
using Gateways.NET.SDK.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.NET.SDK
{
    public class GatewaysController : IBackendFolder
    {
        private readonly GatewaysSDK _sdk;

        public GatewaysController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }

        public string FolderName => "Gateways";

        protected virtual T Respond<T>(ApiResponse<T> apiResponse)
        {
            if (!apiResponse.IsSuccess())
            {
                var error = apiResponse.Errors.FirstOrDefault();
                throw new ApiException(error.Message, error.Code);
            }
            return apiResponse.Payload;
        }

        public async Task<FullGatewayViewModel> AddGateway(GatewayViewModel model)
        {
            var apiResponse = await _sdk.Backend.Post<ApiResponse<FullGatewayViewModel>>(FolderName, model);
            return Respond(apiResponse);
        }

        public async Task<IEnumerable<FullGatewayViewModel>> GetGateways()
        {
            var apiResponse = await _sdk.Backend.Get<ApiResponse<IEnumerable<FullGatewayViewModel>>>(FolderName);
            return Respond(apiResponse);
        }
    }
}
