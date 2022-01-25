using Gateways.NET.SDK.Contracts;

namespace Gateways.NET.SDK
{
    public class PeripheralsController : IBackendFolder
    {
        private readonly GatewaysSDK _sdk;

        public PeripheralsController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }

        public string FolderName => "Peripherals";
    }
}
