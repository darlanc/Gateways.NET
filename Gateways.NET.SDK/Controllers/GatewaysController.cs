using Gateways.NET.Contracts;
using Gateways.NET.CoreViewModels;
using Gateways.NET.SDK.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateways.NET.SDK
{
    public class GatewaysController : ControllerBase
    {
        private readonly GatewaysSDK _sdk;

        public GatewaysController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }

        public override string FolderName => "Gateways";

        
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

        public async Task DeleteGateway(int id)
        {
            await _sdk.Backend.Delete<ApiResponse>($"{FolderName}/{id}");
        }

        public async Task<GatewayViewModel> UpdateGateway(int id, GatewayViewModel model)
        {
            var apiResponse = await _sdk.Backend.Put<ApiResponse<GatewayViewModel>>($"{FolderName}/{id}", model);
            return Respond(apiResponse);
        }

        public async Task<FullGatewayViewModel> GetGateaway(int id)
        {
            var apiResponse = await _sdk.Backend.Get<ApiResponse<FullGatewayViewModel>>($"{FolderName}/{id}");
            return Respond(apiResponse);
        }

        public async Task<PeripheralViewModel> AddPeripheral(int id, PeripheralViewModel model)
        {
            var apiResponse = await _sdk.Backend.Post<ApiResponse<PeripheralViewModel>>($"{FolderName}/{id}/peripherals", model);
            return Respond(apiResponse);
        }

        public async Task<IEnumerable<FullPeripheralViewModel>> GetPeripherals(int id)
        {
            var apiResponse = await _sdk.Backend.Get<ApiResponse<IEnumerable<FullPeripheralViewModel>>>($"{FolderName}/{id}/peripherals");
            return Respond(apiResponse);
        }
    }
}
