using System;

namespace Gateways.NET.SDK
{
    public class GatewaysSDK
    {
        private readonly ApiConfiguration _config;
        private readonly ExtensibleBackendClient _backend;
        private readonly GatewaysController _gateways;

        public GatewaysSDK(ApiConfiguration config)
        {
            _config = config;
            _backend = new ExtensibleBackendClient(config, null);
            Gateways = new GatewaysController(this);
        }

        protected internal ExtensibleBackendClient Backend => _backend;

        public virtual GatewaysController Gateways {get; protected set;}

        public virtual PeripheralsController Peripherals { get; protected set; }
    }
}
