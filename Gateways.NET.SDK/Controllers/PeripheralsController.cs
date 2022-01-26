using Gateways.NET.Contracts;
using Gateways.NET.CoreViewModels;
using Gateways.NET.SDK.Contracts;
using System.Threading.Tasks;

namespace Gateways.NET.SDK
{
    public class PeripheralsController : ControllerBase
    {
        private readonly GatewaysSDK _sdk;

        public PeripheralsController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }

        public override string FolderName => "Peripherals";

        public async Task<FullPeripheralViewModel> AddPeripheral(PeripheralViewModel model)
        {
            var apiResponse = await _sdk.Backend.Post<ApiResponse<FullPeripheralViewModel>>(FolderName, model);
            return Respond(apiResponse);
        }

        public async Task DeletePeripheral(int id)
        {
            await _sdk.Backend.Delete<ApiResponse>($"{FolderName}/{id}");
        }

        public async Task<PeripheralViewModel> UpdatePeripheral(int id, PeripheralViewModel model)
        {
            var apiResponse = await _sdk.Backend.Put<ApiResponse<PeripheralViewModel>>($"{FolderName}/{id}", model);
            return Respond(apiResponse);
        }

        public async Task AttachPeripheral(int id, AttachPeripheralViewModel model)
        {
            await _sdk.Backend.Patch<ApiResponse>($"{FolderName}/{id}/attach", model);            
        }

        public async Task DetachPeripheral(int id)
        {
            await _sdk.Backend.Patch<ApiResponse>($"{FolderName}/{id}/detach", new object());     
        }

        public async Task UpdatePeripheralStatus(int id, PeripheralStatusViewModel model)
        {
            await _sdk.Backend.Patch<ApiResponse>($"{FolderName}/{id}/status", model);
        }
    }
}
