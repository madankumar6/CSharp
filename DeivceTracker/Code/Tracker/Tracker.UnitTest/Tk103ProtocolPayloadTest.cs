using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracker.Common.Model;
using System.Text;

namespace Tracker.UnitTest
{
    [TestClass]
    public class Tk103ProtocolPayloadTest
    {
        [TestMethod]
        public void CanParseData_Type1Device1()
        {
            string payLoad = "(014114564660BP05000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";

            Tracker.Protocol.IProtocol protool = new Tracker.Protocol.Tk103Protocol();
            DeviceInfo di = protool.Parse(new DeviceInfo() { Payload = payLoad });

            Assert.IsTrue(di.DeviceId == "014114564660");
            //Assert.IsTrue(di.CommandType == "BP05");
            Assert.IsTrue(di.DeviceId == "000014114564660");

            //di.TrackerDataActionTime = payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
            Assert.IsTrue(di.ValidData == "A");

            Assert.IsTrue(di.Latitude == "1058.2023N");
            Assert.IsTrue(di.Longitude == "07737.1963E");
            Assert.IsTrue(di.Speed == "000.0");
            Assert.IsTrue(di.TrackerDataActionTime == DateTime.UtcNow);
            Assert.IsTrue(di.Direction == "58.63");
            Assert.IsTrue(di.OnBattery == 0);
            Assert.IsTrue(di.OnIgnition == 0);
            Assert.IsTrue(di.OnAc == 0);
            Assert.IsTrue(di.UnKnown == "");
            Assert.IsTrue(di.OnGps == 0);
            Assert.IsTrue(di.Mileage == "04527024");

        }

        [TestMethod]
        public void CanParseData_Type1Device2()
        {
            string payLoad = "(014114566100BP05000014114566100160122A1051.3956N07841.5273E000.0162601176.72000004F6L02CEA340)";
            //string payLoad = "(014114564660BP05000014114564660160122A1058.2020N07737.1966E000.016255758.63000000178L04527024)"
            Tracker.Protocol.IProtocol protool = new Tracker.Protocol.Tk103Protocol();
            DeviceInfo di = protool.Parse(new DeviceInfo() { Payload = payLoad });

            Assert.IsTrue(di.DeviceId == "014114566100");
            Assert.IsTrue(di.CommandType == DeviceCommandType.None);
            Assert.IsTrue(di.DeviceId == "000014114566100");

            //di.TrackerDataActionTime = payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
            Assert.IsTrue(di.ValidData == "A");

            Assert.IsTrue(di.Latitude == "1051.3956N");
            Assert.IsTrue(di.Longitude == "07841.5273E");
            Assert.IsTrue(di.Speed == "000.0");
            Assert.IsTrue(di.TrackerDataActionTime == DateTime.UtcNow);
            Assert.IsTrue(di.Direction == "176.72");
            Assert.IsTrue(di.OnBattery == 0);
            Assert.IsTrue(di.OnIgnition == 0);
            Assert.IsTrue(di.OnAc == 0);
            Assert.IsTrue(di.UnKnown == "0");
            Assert.IsTrue(di.OnGps == 0);
            Assert.IsTrue(di.Mileage == "02CEA340");

        }

        [TestMethod]
        public void CanParseData_Type1Device3()
        {
            string payLoad = "(014114564660BP05000014114564660160122A1058.2020N07737.1966E000.016255758.63000000178L04527024)";
            Tracker.Protocol.IProtocol protool = new Tracker.Protocol.Tk103Protocol();
            DeviceInfo di = protool.Parse(new DeviceInfo() { Payload = payLoad });

            Assert.IsTrue(di.DeviceId == "014114564660");
            Assert.IsTrue(di.CommandType == DeviceCommandType.None);
            Assert.IsTrue(di.DeviceId == "000014114564660");

            //di.TrackerDataActionTime = payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
            Assert.IsTrue(di.ValidData == "A");
            Assert.IsTrue(di.Latitude == "1058.2020N");
            Assert.IsTrue(di.Longitude == "07737.1966E");
            Assert.IsTrue(di.Speed == "000.0");
            Assert.IsTrue(di.TrackerDataActionTime == DateTime.UtcNow);
            Assert.IsTrue(di.Direction == "58.63");
            Assert.IsTrue(di.OnBattery == 0);
            Assert.IsTrue(di.OnIgnition == 0);
            Assert.IsTrue(di.OnAc == 0);
            Assert.IsTrue(di.UnKnown == "");
            Assert.IsTrue(di.OnGps == 0);
            Assert.IsTrue(di.Mileage == "04527024");

        }
    }
}
