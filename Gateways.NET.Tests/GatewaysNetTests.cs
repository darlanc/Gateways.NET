using Gateways.NET.CoreViewModels;
using Gateways.NET.SDK;
using Gateways.NET.SDK.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Gateways.NET.Tests
{
    [TestClass]
    public class GatewaysNetTests
    {
        protected static ApiConfiguration config = new ApiConfiguration { ApiUrl = "https://localhost:44321/" };
        protected GatewaysSDK sdk = new GatewaysSDK(config);

        protected string GetSerialNumber(int consecutive = 0)
        {
            var date = DateTime.Now;            
            return $"{date.Year}{date.Month}{date.Day}-{date.Hour}{date.Minute}{date.Second}-{consecutive:n4}";
        }

        protected string GetRandomIpv4()
        {
            var rand = new Random();
            return $"{rand.Next(0, 256)}.{rand.Next(0, 256)}.{rand.Next(0, 256)}.{rand.Next(0, 256)}";
        }

        [TestMethod]
        public async Task CheckGatewaysIpv4AddressValidation()
        {
            try
            {
                var model = new GatewayViewModel
                {
                    SerialNumber = GetSerialNumber(),
                    IpAddress = "1.1.1",
                    Name = "Wrong Ipv4"
                };
                await sdk.Gateways.AddGateway(model);
            }
            catch (ApiException ex)
            {
                Assert.IsTrue(ex.Code == (int)HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task AddNewGateway()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(),
                IpAddress = GetRandomIpv4(),
                Name = "Test single Gateway"
            };
            var result = await sdk.Gateways.AddGateway(model);
            Assert.IsTrue(result.SerialNumber == model.SerialNumber && result.IpAddress == model.IpAddress && result.Name == model.Name);
        }

        [TestMethod]
        public async Task CheckUniqueSerialNumberInGateways()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(),
                IpAddress = GetRandomIpv4(),
                Name = "Test unique Gateway"
            };
            await sdk.Gateways.AddGateway(model);

            model.IpAddress = GetRandomIpv4();
            model.Name = "Duplicated Serial Number";

            try
            {
                await sdk.Gateways.AddGateway(model);
            }
            catch (ApiException ex)
            {
                Assert.IsTrue(ex.Code == (int)HttpStatusCode.BadRequest);
            }
        }


    }
}
