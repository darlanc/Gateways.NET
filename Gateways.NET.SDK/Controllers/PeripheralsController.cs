using System;
using Gateways.NET.CoreViewModels;
using Gateways.NET.ViewModels; 

namespace Gateways.NET.SDK
{
    public class PeripheralsController
    {
        private readonly GatewaysSDK _sdk;

        public PeripheralsController(GatewaysSDK sdk)
        {
            _sdk = sdk;
        }
    }
}
