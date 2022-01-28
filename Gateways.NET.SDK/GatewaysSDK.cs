using System;

namespace Gateways.NET.SDK
{
    public class GatewaysSDK
    {
        public GatewaysSDK(ApiConfiguration config)
        {
            Config = config;
            Backend = new ExtensibleBackendClient(config, null);
            Gateways = new GatewaysController(this);
            Peripherals = new PeripheralsController(this);
        }

        protected internal ExtensibleBackendClient Backend { get; protected set; }

        public virtual GatewaysController Gateways { get; protected set; }

        public virtual PeripheralsController Peripherals { get; protected set; }

        public virtual ApiConfiguration Config { get; protected set; }
    }
}
