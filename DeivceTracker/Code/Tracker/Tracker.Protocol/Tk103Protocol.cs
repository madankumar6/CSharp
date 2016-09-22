using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Common.Model;

namespace Tracker.Protocol
{
    public class Tk103Protocol : IProtocol
    {
        const int REQ_MIN_Len = 10;
        const int REQ_MAX_Len = 10;

        public int REQ_MIN_Length()
        {
            return REQ_MIN_Len;
        }

        public int REQ_MAX_Length()
        {
            return REQ_MAX_Len;
        }

        public DeviceInfo Parse(DeviceInfo deviceInfo)
        {
            deviceInfo.Payload += (Encoding.ASCII.GetString(deviceInfo.RawData, 0, deviceInfo.RawData.Length));

            Console.WriteLine("Tk103Protocol, Payload: {0}, PayloadLength: {1}", deviceInfo.Payload, deviceInfo.Payload.Length);
            //deviceInfo.PayLoadMinLength = 94;
            if (deviceInfo.Payload.IndexOf('(') < deviceInfo.Payload.IndexOf(')'))
            {

                //deviceInfo.PayLoad = "(014114564660BP05000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1); //Eliminating (

                deviceInfo.DeviceId = deviceInfo.Payload.Substring(0, 12); // 014114564660 BP05000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(12);

                deviceInfo.CommandType = DeviceCommandType.LocationData;// deviceInfo.PayLoad.Substring(0, 4); //BP05 000014114564660160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                deviceInfo.DeviceId = deviceInfo.Payload.Substring(0, 15); //000014114564660 160122A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(15);

                // Parse body
                // Datetime in SQL understan format '2015-01-23 12:23:23'
                 string dt = DateTime.UtcNow.Date.Year.ToString().Substring(0, 2) + deviceInfo.Payload.Substring(0, 6); //160122 A1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                // di.TrackerDataActionTime 20160122
                dt = dt.Insert(6, "-");
                dt = dt.Insert(4, "-") + " ";
                deviceInfo.Payload = deviceInfo.Payload.Substring(6);

                deviceInfo.ValidData = deviceInfo.Payload.Substring(0, 1); //A 1058.2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                //TODO Lat
                deviceInfo.Latitude = deviceInfo.Payload.Substring(0, deviceInfo.Payload.IndexOf('.') + 1); //1058. 2023N07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Latitude += deviceInfo.Payload.Substring(deviceInfo.Latitude.Length, 5); //1058. 2023N 07737.1963E000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Latitude.Length);

                //TODO Long
                deviceInfo.Longitude = deviceInfo.Payload.Substring(0, deviceInfo.Payload.IndexOf('.') + 1); //07737. 1963E000.016260558.63000000178L04527024)";
                deviceInfo.Longitude += deviceInfo.Payload.Substring(deviceInfo.Longitude.Length, 5); //07737. 1963E 000.016260558.63000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Longitude.Length);

                deviceInfo.Speed = deviceInfo.Payload.Substring(0, deviceInfo.Payload.IndexOf('.') + 1);
                deviceInfo.Speed += deviceInfo.Payload.Substring(deviceInfo.Speed.Length, 1); //000. 0 16260558.63000000178L04527024)";            
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Speed.Length);

                // TODO Proper datetime onvertion
                // 2015-01-23 12:23:23 already cotaind 2016-01-22
                dt += deviceInfo.Payload.Substring(0, 6); //162605 58.63000000178L04527024)";
                dt = dt.Insert(13, ":");
                dt = dt.Insert(16, ":");
                deviceInfo.TrackerDataActionTime = DateTime.Parse(dt);
                deviceInfo.Payload = deviceInfo.Payload.Substring(6);

                deviceInfo.Direction = deviceInfo.Payload.Substring(0, deviceInfo.Payload.IndexOf('.') + 1); //58. 63000000178L04527024)";
                deviceInfo.Direction += deviceInfo.Payload.Substring(deviceInfo.Direction.Length, 2); //63 000000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Direction.Length);

                deviceInfo.OnBattery = int.Parse(deviceInfo.Payload.Substring(0, 1)); //0 00000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                deviceInfo.OnIgnition = int.Parse(deviceInfo.Payload.Substring(0, 1));//0 0000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                deviceInfo.OnAc = int.Parse(deviceInfo.Payload.Substring(0, 1));//0 000178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                deviceInfo.UnKnown = deviceInfo.Payload.Substring(0, 1);//0 00178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                deviceInfo.OnGps = int.Parse(deviceInfo.Payload.Substring(0, 1));//0 0178L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(1);

                //deviceInfo.PayLoad = deviceInfo.PayLoad.Substring(1);//0 178L04527024)";
                //deviceInfo.PayLoad = deviceInfo.PayLoad.Substring(1);//1 78L04527024)";
                //deviceInfo.PayLoad = deviceInfo.PayLoad.Substring(1);//7 8L04527024)";
                //deviceInfo.PayLoad = deviceInfo.PayLoad.Substring(1);//8 L04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Payload.IndexOf('L') + 1);
                //TODO: Hexcode onvertions
                deviceInfo.Mileage = deviceInfo.Payload.Substring(0, deviceInfo.Payload.IndexOf(')'));//L 04527024)";
                deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Payload.IndexOf(')') + 1);

                //deviceInfo.ReplyMessage = "ACO0";
            }
            else
            {
            }
            deviceInfo = SetDataParsedTime(deviceInfo);
            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
            return deviceInfo;
        }
        public DeviceInfo SetDataParsedTime(DeviceInfo di)
        {
            di.TrackerDataParsedTime = DateTime.UtcNow;
            return di;
        }
    }
}
