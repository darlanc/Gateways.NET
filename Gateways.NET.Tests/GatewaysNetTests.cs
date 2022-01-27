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

        #region [ Support Methods ]

        protected uint gatewaysInnerCount = 0;
        protected uint peripheralsInnerCount = 0;

        protected string GetSerialNumber(int consecutive = 0)
        {
            var date = DateTime.Now;
            gatewaysInnerCount++;
            return $"{date.Year}{date.Month}{date.Day}-{date.Hour}{date.Minute}{date.Second}-{(gatewaysInnerCount + consecutive):n4}";
        }

        protected string GetRandomIpv4()
        {
            var rand = new Random();
            return $"{rand.Next(0, 256)}.{rand.Next(0, 256)}.{rand.Next(0, 256)}.{rand.Next(0, 256)}";
        }

        protected uint GetUID(uint consecutive = 0)
        {
            var date = DateTime.Now;
            var startDate = new DateTime(2022, 1, 26);
            var ticks = date.Ticks - startDate.Ticks;
            peripheralsInnerCount++;
            return (uint)ticks + consecutive + peripheralsInnerCount;
        }

        public void AssertAreEquals(PeripheralViewModel source, PeripheralViewModel target)
        {
            Assert.AreEqual(source.UID, target.UID);
            Assert.AreEqual(source.Vendor, target.Vendor);
            Assert.AreEqual(source.Status, target.Status);
        }

        #endregion

        #region < Gateways Test Methods >

        #region [ IP validation ]

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

        #endregion

        #region [ Add Gateway ]

        [TestMethod]
        public async Task AddNewGateway()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(1),
                IpAddress = GetRandomIpv4(),
                Name = "Test single Gateway"
            };
            var result = await sdk.Gateways.AddGateway(model);
            Assert.IsTrue(result.SerialNumber == model.SerialNumber && result.IpAddress == model.IpAddress && result.Name == model.Name);
        }

        #endregion

        #region [ Unique Serial Number ]

        [TestMethod]
        public async Task CheckUniqueSerialNumberInGateways()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(10),
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

        #endregion

        #region [ Update Gateway ]

        [TestMethod]
        public async Task CheckUpdateGateway()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(2),
                IpAddress = GetRandomIpv4(),
                Name = "Test update Gateway"
            };
            var result = await sdk.Gateways.AddGateway(model);
            var id = result.Id;

            model.Name = "Updated Gateway name";

            var updated = await sdk.Gateways.UpdateGateway(id, model);
            Assert.AreEqual(model.Name, updated.Name);
        }

        #endregion

        #region [ Add new peripheral ]

        [TestMethod]
        public async Task CheckAddSingleNewPeripheralToGateway()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(3),
                IpAddress = GetRandomIpv4(),
                Name = "Test single peripheral Gateway"
            };
            var gateway = await sdk.Gateways.AddGateway(model);
            var id = gateway.Id;

            var peripheralModel = new PeripheralViewModel
            {
                UID = GetUID(),
                Vendor = "Musala Soft",
                Status = true
            };

            var result = await sdk.Gateways.AddPeripheral(id, peripheralModel);
            AssertAreEquals(peripheralModel, result);

            var parentGateway = await sdk.Gateways.GetGateaway(id);
            Assert.IsTrue(parentGateway.Peripherals.Length == 1);
            AssertAreEquals(parentGateway.Peripherals[0], peripheralModel);
        }

        #endregion

        #region [ Peripherals Max Limit ]

        [TestMethod]
        public async Task CheckPeripheralsLimit()
        {
            var model = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(4),
                IpAddress = GetRandomIpv4(),
                Name = "Test full Gateway"
            };
            var gateway = await sdk.Gateways.AddGateway(model);
            var id = gateway.Id;

            //Add 10 peripherals (max)
            for (uint i = 0; i < 10; i++)
            {
                var peripheralModel = new PeripheralViewModel
                {
                    UID = GetUID(i+1),
                    Vendor = "Musala Soft",
                    Status = true
                };
                await sdk.Gateways.AddPeripheral(id, peripheralModel);
            }

            //Check peripherals list of the Gateway
            var parentGateway = await sdk.Gateways.GetGateaway(id);
            Assert.IsTrue(parentGateway.Peripherals.Length == 10);

            try
            {
                // Attempt to add a new peripheral over the limit
                var peripheralModel = new PeripheralViewModel
                {
                    UID = GetUID(11),
                    Vendor = "Musala Soft",
                    Status = true
                };
                await sdk.Gateways.AddPeripheral(id, peripheralModel);
                throw new Exception("Test failed, the new peripheral was added over the limit");
            }
            catch (ApiException ex)
            {
                Assert.IsTrue(ex.Code == (int)HttpStatusCode.BadRequest);
            }
        }

        #endregion

        #endregion

        #region < Peripherals Test Methods >

        #region [ Add Peripheral ]

        [TestMethod]
        public async Task AddUpdateNewPeripheral()
        {
            var model = new PeripheralViewModel
            {
                UID = GetUID(1),
                Vendor = "Musala Soft",
                Status = true
            };

            var result = await sdk.Peripherals.AddPeripheral(model);
            AssertAreEquals(model, result);

            model.Vendor = "Darlanc Corp.";
            var updatedResult = await sdk.Peripherals.UpdatePeripheral(result.Id, model);
            AssertAreEquals(model, updatedResult);
        }

        [TestMethod]
        public async Task CheckAttachDetachPeripheral()
        {
            var gatewayModel = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(5),
                IpAddress = GetRandomIpv4(),
                Name = "Attach/detach peripheral Gateway"
            };

            var peripheralModel = new PeripheralViewModel
            {
                UID = GetUID(2),
                Vendor = "Musala Soft",
                Status = true
            };

            var gateway = await sdk.Gateways.AddGateway(gatewayModel);
            var peripheral = await sdk.Peripherals.AddPeripheral(peripheralModel);

            await sdk.Peripherals.AttachPeripheral(peripheral.Id, new AttachPeripheralViewModel { GatewayId = gateway.Id });

            var updatedGateway = await sdk.Gateways.GetGateaway(gateway.Id);
            Assert.IsTrue(updatedGateway.Peripherals.Length == 1);
            AssertAreEquals(peripheral, updatedGateway.Peripherals[0]);

            await sdk.Peripherals.DetachPeripheral(peripheral.Id);
            updatedGateway = await sdk.Gateways.GetGateaway(gateway.Id);
            Assert.IsTrue(updatedGateway.Peripherals.Length == 0);
        }

        [TestMethod]
        public async Task CheckChangePeripheralStatus()
        {
            var gatewayModel = new GatewayViewModel
            {
                SerialNumber = GetSerialNumber(),
                IpAddress = GetRandomIpv4(),
                Name = "On/Off peripheral Gateway"
            };

            var peripheralModel = new PeripheralViewModel
            {
                UID = GetUID(3),
                Vendor = "Musala Soft",
                Status = true
            };

            var gateway = await sdk.Gateways.AddGateway(gatewayModel);
            var peripheral = await sdk.Peripherals.AddPeripheral(peripheralModel);
            await sdk.Peripherals.AttachPeripheral(peripheral.Id, new AttachPeripheralViewModel { GatewayId = gateway.Id });

            await sdk.Peripherals.UpdatePeripheralStatus(peripheral.Id, new PeripheralStatusViewModel { Status = false });
            var updatedGateway = await sdk.Gateways.GetGateaway(gateway.Id);
            Assert.IsFalse(updatedGateway.Peripherals[0].Status);
        }


        #endregion

        #endregion
    }


}

