using System;

namespace Gateways.NET.SDK
{
    public class GatewaysController
    {
        private readonly GatewaysSDK _sdk;

        public GatewaysController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }
    }
}
