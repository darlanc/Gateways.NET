using Gateways.NET.Contracts;
using Gateways.NET.SDK.Contracts;
using System;
using System.Net;
using System.Threading.Tasks;

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

        public virtual async Task<bool> IsServiceAvailable()
        {
            try
            {
                var result = await Backend.GetStatusCode("System");
                return result == HttpStatusCode.NoContent;
            }
            catch (ApiException ex)
            {
                if (ex.Code == (int)HttpStatusCode.ServiceUnavailable)
                    return false;
                throw ex;
            }
        }
    }
}
