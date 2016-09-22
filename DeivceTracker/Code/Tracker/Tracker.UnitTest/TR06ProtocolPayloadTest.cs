using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracker.Common.Model;
using System.Text;

namespace Tracker.UnitTest
{
    [TestClass]
    public class TR06ProtocolPayloadTest
    {
        [TestMethod]
        public void CanParseData_LoginMessage()
        {
            byte[] payloadByte = { 0x78, 0x78, 0x0D, 0x01, 0x01, 0x23, 0x45, 0x67, 0x89, 0x01, 0x23, 0x45, 0x00, 0x01, 0x8C, 0xDD, 0x0D, 0x0A };

            //string payLoad = "(014114564660BP05000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";

            Tracker.Protocol.IProtocol protool = new Tracker.Protocol.Tr06Protocol();
            DeviceInfo di = protool.Parse(new DeviceInfo() { RawData = payloadByte });

            //Assert.IsTrue(di.TrackerId == "014114564660");
            //Assert.IsTrue(di.CommandType == "BP05");
            //Assert.IsTrue(di.DeviceId == "000014114564660");

            ////di.TrackerDataActionTime = payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
            //Assert.IsTrue(di.ValidData == "A");

            //Assert.IsTrue(di.Latitude == "1058.2023N");
            //Assert.IsTrue(di.Longitude == "07737.1963E");
            //Assert.IsTrue(di.Speed == "000.0");
            //Assert.IsTrue(di.TrackerDataActionTime == "2016-01-22 16:26:05");
            //Assert.IsTrue(di.Direction == "58.63");
            //Assert.IsTrue(di.OnBattery == "0");
            //Assert.IsTrue(di.OnIgnition == "0");
            //Assert.IsTrue(di.OnAc == "0");
            //Assert.IsTrue(di.UnKnown == "0");
            //Assert.IsTrue(di.OnGps == "0");
            //Assert.IsTrue(di.Mileage == "04527024");

        }
        [TestMethod]
        public void CanParseData_LocationMessage()
        {
            byte[] payloadByte = { 0x78, 0x78, 0x1F, 0x12, 0x0B, 0x08, 0x1D, 0x11, 0x2E, 0x10, 0xCC, 0x02, 0x7A, 0xC7, 0xEB, 0x0C, 0x46, 0x58, 0x49, 0x00, 0x14, 0x8F, 0x01, 0xCC, 0x00, 0x28, 0x7D, 0x00, 0x1F, 0xB8, 0x00, 0x03, 0x80, 0x81, 0x0D, 0x0A };

            //string payLoad = "(014114564660BP05000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";

            Tracker.Protocol.IProtocol protool = new Tracker.Protocol.Tr06Protocol();
            DeviceInfo di = protool.Parse(new DeviceInfo() { RawData = payloadByte });

            //Assert.IsTrue(di.TrackerId == "014114564660");
            //Assert.IsTrue(di.CommandType == "BP05");
            //Assert.IsTrue(di.DeviceId == "000014114564660");

            ////di.TrackerDataActionTime = payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
            //Assert.IsTrue(di.ValidData == "A");

            //Assert.IsTrue(di.Latitude == "1058.2023N");
            //Assert.IsTrue(di.Longitude == "07737.1963E");
            //Assert.IsTrue(di.Speed == "000.0");
            //Assert.IsTrue(di.TrackerDataActionTime == "2016-01-22 16:26:05");
            //Assert.IsTrue(di.Direction == "58.63");
            //Assert.IsTrue(di.OnBattery == "0");
            //Assert.IsTrue(di.OnIgnition == "0");
            //Assert.IsTrue(di.OnAc == "0");
            //Assert.IsTrue(di.UnKnown == "0");
            //Assert.IsTrue(di.OnGps == "0");
            //Assert.IsTrue(di.Mileage == "04527024");

        }
    }
}
