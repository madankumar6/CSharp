using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Script.Serialization;
using Tracker.Common.Model;

using Utils;

namespace Tracker.Parser
{
    public class GT300Protocol : Protocol
    {
        public struct ProtocolNumber
        {
            public const String LoginMessage = "01";
            public const String LBSInformationPacket = "22";
            public const String StatusInformationPacket = "13";
            public const String StringInformationPacket = "21";
            public const String LBSCheckingLocationViaPhoneNumberInformationPacket = "17";
            public const String LBSStatusMergedPacket = "19";
            public const String LBSMultibaseStationInformationPacket = "28";
            public const String AlarmData = "26";
            public const String AlarmDatawithMultipleFences = "27";
            public const String GPSCheckingLocationViaPhoneNumberInformationPacket = "2A";
            public const String ServerSendsCommandtoTerminal = "80";
        }

        public struct PayloadLength
        {
            public const int LoginMessage = 22;
            public const int LBSGPSMergedPacket = 39;
            public const int LBSExtensionPacket = 46;
            public const int AlarmData = 43;
            public const int StatusInformation = 15; // hearbeat
            public const int LBSPhonenumbercheckingLocationInfoPackage = 39;
            public const int LBSStatusInfoPackage = 23;
        }
        public override int REQ_MIN_Length()
        {
            return PayloadLength.StatusInformation;
        }
        public override DeviceInfo Parse(DeviceInfo deviceInfo)
        {
            try
            {
                deviceInfo.Payload += BitConverter.ToString(deviceInfo.RawData).Replace("-", string.Empty);
                deviceInfo.RawData = new byte[0];
                var tPayload = deviceInfo.Payload;
                log.DebugFormat("Device IP:{0},payload:{1},deviceInfo.Trackerip,deviceInfo.payload");
                if (deviceInfo.Payload.Length < REQ_MIN_Length())
                {
                    deviceInfo.ParserStatus = ProtocolParserStatus.InSufficientData;
                    return deviceInfo;
                }
                if ((ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4)) == "7878")
                {
                    int PacketLength = GetNumber((ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2)));
                    string packetComparer = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);

                    if (packetComparer == ProtocolNumber.LoginMessage)
                    {
                        if (tPayload.Length >= (PayloadLength.LoginMessage * 2))
                        {
                            deviceInfo.DeviceId = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 16);
                            deviceInfo.CommandType = DeviceCommandType.LoginData;
                            byte[] forCheckSumData ={
                                0x05,0x05,0x01,
                                (byte)(Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2), 16)),
                                (byte)(Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2), 16))
                            };
                            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);
                            byte[] payloadByte = { 0x78 };
                            payloadByte = payloadByte.Concat(forCheckSumData).ToArray();
                            payloadByte = payloadByte.Concat(checkSum).ToArray();
                            byte[] endValue = { 0x0D, 0x0A };
                            payloadByte = payloadByte.Concat(endValue).ToArray();
                            deviceInfo.ToSendRawData = payloadByte;
                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.LoginMessage * 2);
                            log.DebugFormat("{0}/Parse data: {1}", "GT300Protocol", payloadByte);
                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }

                    }

                    else if (packetComparer == ProtocolNumber.LBSInformationPacket)
                    {
                        if (tPayload.Length >= (PayloadLength.LBSGPSMergedPacket * 2))
                        {
                            deviceInfo.CommandType = DeviceCommandType.LocationData;

                            var datetime = DateTime.UtcNow.Year.ToString().Substring(0, 2) + GetNumberString(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 12));

                            deviceInfo.DeviceDataTime = new DateTime(Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 4)), Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 2)), Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 2)),
                                Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 2)), Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 2)), Convert.ToInt32(ParserHelper.TSubstring(ref datetime, 0, 2)));

                            // Parse Sinal 
                            var gsQty = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);
                            // Parse lat
                            //0x02 0x6B 0x3F 0x3E
                            // To 40582974(Decimal)= 26B3F3E(Hexadecimal)
                            //22º32.7658‟=(22X60+32.7658)X30000=40582974

                            var latMin = (Double)Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 8), 16);
                            var Latitude = (latMin / Double.Parse("60") / Double.Parse("30000")).ToString("0.000000000").Replace("-", ""); // To device Minute value

                            var longMin = (Double)Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 8), 16);
                            var Longitude = (longMin / Double.Parse("60") / Double.Parse("30000")).ToString("0.000000000").Replace("-", ""); // To device Minute value

                            var Speed = GetNumberString(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2)).ToString();


                            deviceInfo.Odometer = //(deviceInfo.Odometer ?? 0) +
                                Convert.ToInt32(GetDistance(deviceInfo.Latitude, deviceInfo.Longitude, Latitude, Longitude, deviceInfo.Speed.ToNullable<int>(), Speed.ToNullable<int>()));

                            deviceInfo.Speed = Speed;
                            deviceInfo.Latitude = Latitude;
                            deviceInfo.Longitude = Longitude;

                            // Lat Long East / Nort calc
                            int courseS = GetNumber(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4));
                            if ((8192 & courseS) != 0)
                            {
                                //real time position
                            }
                            else
                            {
                                // differential position
                            }

                            if ((4096 & courseS) != 0)
                            {
                                //GPS positioned
                            }
                            else
                            {
                                //GPS not positioned
                            }


                            if ((2048 & courseS) != 0)
                            {
                                //West Longitude
                                // Add (-) to Longitude
                                deviceInfo.Longitude = "-" + deviceInfo.Longitude;
                            }
                            else
                            {
                                //East Longitude
                            }

                            if ((1024 & courseS) != 0)
                            {
                                //North Latitude
                            }
                            else
                            {
                                //South Latitude
                                deviceInfo.Latitude = "-" + deviceInfo.Latitude;
                            }

                            deviceInfo.Direction = (1023 & courseS).ToString();

                            var mcc = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4);
                            var mnc = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);
                            var lac = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4);
                            var cellId = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 6);
                            var ACC = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);
                            var DataReportingMode = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);
                            var GPSRealTimeResend = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);
                            var serialNo = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4);
                            object obj = new
                            {
                                mcc = mcc,
                                mnc = mnc,
                                lac = lac,
                                cellId = cellId,
                                ACC = ACC,
                                DataReportingMode = DataReportingMode,
                                GPSRealTimeResend = GPSRealTimeResend,
                                serialNo = serialNo
                            };

                            //byte[] payloadByte = { 0x78, 0x78, 0x05, 0x01, 0x00, 0x01, 0xD9, 0xDC, 0x0D, 0x0A };
                            //deviceInfo.TrackerSendPayLoadBytes = payloadByte;
                            deviceInfo.UnKnown = new JavaScriptSerializer().Serialize(obj);
                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.LBSGPSMergedPacket * 2);
                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }
                    }

                    else if (packetComparer == ProtocolNumber.AlarmData)
                    {
                        if (tPayload.Length >= (PayloadLength.AlarmData * 2))
                        {

                            deviceInfo.CommandType = DeviceCommandType.AlarmData;

                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 12); // Datetime
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2); // gis
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 8); // lat
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 8); // long
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2); // speed
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4); // course
                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 18); // LBS



                            string statusInfo = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 10);

                            int tic = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            //128:	1000 0000-> true: oil & electricity disconnected, else working
                            deviceInfo.OilNElectricConected = (128 & tic) != 0 ? 0 : 1; // it is reverse when compared to below

                            //64:	0100 0000-> true: GPS on, GPS off
                            deviceInfo.OnGps = (64 & tic) != 0 ? 1 : 0;

                            //0000 0000
                            //32:		0010 0000-> true: SOS alarm
                            deviceInfo.OnSOS = (32 & tic) != 0 ? 1 : 0;
                            //24:		0001 1000->low battery alarm
                            deviceInfo.OnLowBattery = (24 & tic) != 0 ? 1 : 0;
                            //16:		0001 0000->Power cut alarm
                            deviceInfo.OnPowerCut = (16 & tic) != 0 ? 1 : 0;
                            //8:		0000 1000->shock alarm
                            deviceInfo.OnShock = (8 & tic) != 0 ? 1 : 0;
                            //0:		0000 0000->normal

                            //4:	0000 0100 true charge is on, else off
                            deviceInfo.OnCharge = (4 & tic) != 0 ? 1 : 0;
                            //2:	0000 0010 true ACC high, else low
                            deviceInfo.OnAcc = (2 & tic) != 0 ? 1 : 0;
                            //1:	0000 0001 true defense activated
                            deviceInfo.OnDefence = (1 & tic) != 0 ? 1 : 0;



                            deviceInfo.VoltageLevel = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            deviceInfo.SignalStrengthLevel = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            string alarmLanguage = ParserHelper.TSubstring(ref statusInfo, 0, 2);

                            deviceInfo.AlarmType = DeviceAlarmType.NormalAlarm;
                            if (alarmLanguage == "01")
                                deviceInfo.AlarmType = DeviceAlarmType.SOSAlarm;
                            if (alarmLanguage == "02")
                                deviceInfo.AlarmType = DeviceAlarmType.PowerCutAlarm;
                            if (alarmLanguage == "03")
                                deviceInfo.AlarmType = DeviceAlarmType.VibrationAlarm;
                            if (alarmLanguage == "04")
                                deviceInfo.AlarmType = DeviceAlarmType.FenceInAlarm;
                            if (alarmLanguage == "05")
                                deviceInfo.AlarmType = DeviceAlarmType.FenceOutAlarm;
                            if (alarmLanguage == "06")
                                deviceInfo.AlarmType = DeviceAlarmType.SpeedAlarm;
                            if (alarmLanguage == "09")
                                deviceInfo.AlarmType = DeviceAlarmType.MovingAlarm;

                            ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 18);

                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.AlarmData * 2);

                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }
                    }
                    else if (packetComparer == ProtocolNumber.StatusInformationPacket)
                    {
                        if (tPayload.Length >= (PayloadLength.StatusInformation * 2))
                        {
                            deviceInfo.CommandType = DeviceCommandType.StatusInformation;
                            string statusInfo = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 10);
                            int tic = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            //128:	1000 0000-> true: oil & electricity disconnected, else working
                            deviceInfo.OilNElectricConected = (128 & tic) != 0 ? 0 : 1; // it is reverse when compared to below

                            //64:	0100 0000-> true: GPS on, GPS off
                            deviceInfo.OnGps = (64 & tic) != 0 ? 1 : 0;

                            //0000 0000
                            //32:		0010 0000-> true: SOS alarm
                            deviceInfo.OnSOS = (32 & tic) != 0 ? 1 : 0;
                            //24:		0001 1000->low battery alarm
                            deviceInfo.OnLowBattery = (24 & tic) != 0 ? 1 : 0;
                            //16:		0001 0000->Power cut alarm
                            deviceInfo.OnPowerCut = (16 & tic) != 0 ? 1 : 0;
                            //8:		0000 1000->shock alarm
                            deviceInfo.OnShock = (8 & tic) != 0 ? 1 : 0;
                            //0:		0000 0000->normal

                            //4:	0000 0100 true charge is on, else off
                            deviceInfo.OnCharge = (4 & tic) != 0 ? 1 : 0;
                            //2:	0000 0010 true ACC high, else low
                            deviceInfo.OnAcc = (2 & tic) != 0 ? 1 : 0;
                            //1:	0000 0001 true defense activated
                            deviceInfo.OnDefence = (1 & tic) != 0 ? 1 : 0;



                            deviceInfo.VoltageLevel = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            deviceInfo.SignalStrengthLevel = GetNumber(ParserHelper.TSubstring(ref statusInfo, 0, 2));

                            string alarmLanguage = ParserHelper.TSubstring(ref statusInfo, 0, 2);

                            deviceInfo.AlarmType = DeviceAlarmType.NormalAlarm;
                            if (alarmLanguage == "01")
                                deviceInfo.AlarmType = DeviceAlarmType.SOSAlarm;
                            if (alarmLanguage == "02")
                                deviceInfo.AlarmType = DeviceAlarmType.PowerCutAlarm;
                            if (alarmLanguage == "03")
                                deviceInfo.AlarmType = DeviceAlarmType.VibrationAlarm;
                            if (alarmLanguage == "04")
                                deviceInfo.AlarmType = DeviceAlarmType.FenceInAlarm;
                            if (alarmLanguage == "05")
                                deviceInfo.AlarmType = DeviceAlarmType.FenceOutAlarm;
                            if (alarmLanguage == "06")
                                deviceInfo.AlarmType = DeviceAlarmType.SpeedAlarm;
                            if (alarmLanguage == "09")
                                deviceInfo.AlarmType = DeviceAlarmType.MovingAlarm;

                            //Skipp latter byte
                            ParserHelper.TSubstring(ref statusInfo, 0, 2);

                            byte[] forCheckSumData = { 0x05,
                                0x13,
                                (byte)(Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2), 16)),
                                (byte)(Convert.ToInt32(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2), 16)),
                            };

                            var checkSum = CRC16.ComputeChecksumBytes(forCheckSumData);

                            byte[] payloadByte = { 0x78, 0x78 };
                            payloadByte = payloadByte.Concat(forCheckSumData).ToArray();
                            payloadByte = payloadByte.Concat(checkSum).ToArray();
                            byte[] endValue = { 0x0D, 0x0A };
                            payloadByte = payloadByte.Concat(endValue).ToArray();

                            deviceInfo.ToSendRawData = payloadByte;

                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.StatusInformation * 2);

                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }
                    }
                    else
                    {
                        deviceInfo.CommandType = DeviceCommandType.UnknownData;
                        // TODO parse Status info
                        deviceInfo.Payload = deviceInfo.Payload.Substring(deviceInfo.Payload.IndexOf("0D0A") + 4);

                        deviceInfo = SetDataParsedTime(deviceInfo);
                        deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                    }
                }
                else
                {
                    // Invalid message
                }

                log.DebugFormat("Device IP: {0}, payLoad parsed", deviceInfo.TrackerIp);

            }
            catch (Exception ex)
            {
                log.ErrorFormat("Parse Error: {0}", ex);
            }
            return deviceInfo;

        }

    }
}


