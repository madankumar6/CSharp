using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Common.Model;
using Utils;

namespace Tracker.Protocol
{
    public class Tr06Protocol : IProtocol
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

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

        private int GetNumber(string twoDigitHexString)
        {
            return Int32.Parse(twoDigitHexString, System.Globalization.NumberStyles.HexNumber);
        }

        private string GetNumberString(string hexString)
        {
            string numberStr = "";
            while (hexString.Length > 1)
            {
                numberStr += GetNumber(hexString.Substring(0, 2)).ToString();
                hexString = hexString.Substring(2);
            }
            if (hexString.Length > 0)
            {
                numberStr += GetNumber(hexString).ToString();
            }
            return numberStr;
        }

        public DeviceInfo Parse(DeviceInfo deviceInfo)
        {
            deviceInfo.Payload += BitConverter.ToString(deviceInfo.RawData).Replace("-", string.Empty);

            log.DebugFormat("Device IP: {0}, payLoad: {1}", deviceInfo.TrackerIp, deviceInfo.Payload);

            if (deviceInfo.Payload.Length < REQ_MIN_Length())
            {
                deviceInfo.ParserStatus = ProtocolParserStatus.InSufficientData;
                return deviceInfo;
            }

            if (deviceInfo.Payload.Substring(0, 4) == "7878")
            {
                deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                int packetLength = GetNumber(deviceInfo.Payload.Substring(0, 2));
                deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                if (deviceInfo.Payload.Substring(0, 2) == "01")
                {
                    // Login message

                    deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                    deviceInfo.DeviceId = deviceInfo.Payload.Substring(0, 16);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(16);

                    byte[] payloadByte = { 0x78, 0x78, 0x05, 0x01, 0x00, 0x01, 0xD9, 0xDC, 0x0D, 0x0A };
                    deviceInfo.ToSendRawData = payloadByte;
                }
                else if (deviceInfo.Payload.Substring(0, 2) == "12")
                {
                    deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                    var datetime = DateTime.UtcNow.Year.ToString().Substring(0, 2);
                    datetime += GetNumberString(deviceInfo.Payload.Substring(0, 12));
                    datetime = datetime.Insert(6, "-");
                    datetime = datetime.Insert(4, "-") + " ";

                    deviceInfo.Payload = deviceInfo.Payload.Substring(12);

                    // Parse Sinal 

                    var gsQty = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 2);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                    // Parse lat
                    //0x02 0x6B 0x3F 0x3E
                    // To 40582974(Decimal)= 26B3F3E(Hexadecimal)
                    //22º32.7658‟=(22X60+32.7658)X30000=40582974

                    var latDec = Decimal.Parse(GetNumberString(deviceInfo.Payload.Substring(0, 8)));
                    decimal latDecN = latDec / decimal.Parse("30000");
                    decimal latDegT = latDecN / decimal.Parse("60");
                    decimal latDeg = Math.Truncate(latDegT);
                    string lat = latDeg.ToString() + ((latDegT - latDeg) * decimal.Parse("60")).ToString("00.0000");

                    //To convert Seconds to decimals, divide by 3600.
                    //Decimal value = Seconds/3600
                    //As an example, a latitude of 122 degrees 45 minutes 45 seconds North is equal to 122.7625 degrees North. 

                    //So the complete formula looks similar the following:
                    //Decimal value = Degrees + (Minutes/60) + (Seconds/3600)

                    deviceInfo.Latitude = lat;
                    deviceInfo.Payload = deviceInfo.Payload.Substring(8);

                    var lanDec = Decimal.Parse(GetNumberString(deviceInfo.Payload.Substring(0, 8)));
                    decimal lanDecN = latDec / decimal.Parse("30000");
                    decimal lanDegT = latDecN / decimal.Parse("60");
                    decimal lanDeg = Math.Truncate(latDegT);
                    string longt = latDeg.ToString() + ((latDegT - latDeg) * decimal.Parse("60")).ToString("00.0000");

                    deviceInfo.Longitude = longt;
                    //var lan = deviceInfo.PayLoad = deviceInfo.PayLoad.Substring(0, 8);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(8);

                    var speed = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 2);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                    var cStatus = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 4);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                    var mcc = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 4);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                    var mnc = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 2);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(2);

                    var lac = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 4);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                    var cellId = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 6);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(6);

                    var serialNo = deviceInfo.Payload = deviceInfo.Payload.Substring(0, 4);
                    deviceInfo.Payload = deviceInfo.Payload.Substring(4);

                    //byte[] payloadByte = { 0x78, 0x78, 0x05, 0x01, 0x00, 0x01, 0xD9, 0xDC, 0x0D, 0x0A };
                    //deviceInfo.TrackerSendPayLoadBytes = payloadByte;
                }

            }
            else
            {
                // Invalid message
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
