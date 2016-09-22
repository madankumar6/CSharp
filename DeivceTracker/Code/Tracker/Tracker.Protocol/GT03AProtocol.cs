using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Tracker.Common.Model;
using Tracker.Parser;
using Utils;


namespace Tracker.Parser
{
    public class GT03AProtocol : Protocol
    {
        public struct ProtocolNumber
        {
            public const string LoginMessage = "01";
            public const string GPSInformationdata = "10";
            public const string LBSInformationdata = "11";
            public const string StatusInformation = "13";
            public const string GPSLBSStatusMergedInformation = "16";
            public const string LBSCheckingLocationViaPhoneNumberInformation = "17";
            public const string LBSstatusmergedpackage = "19";
            public const string GPSCheckingLocationViaPhoneNumberInformation = "26";
            public const string Serversendcommandtoterminal = "80";
        }
        public struct PayloadLength
        {
            public const int LoginMessage = 20;
            public const int GPSInformation = 30;
            public const int LBSInformation = 26;
            public const int StatusInformation = 15;
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
                Console.WriteLine(tPayload);
                log.DebugFormat("Device IP: {0}, payLoad: {1}", deviceInfo.TrackerIp, deviceInfo.Payload);


                if (deviceInfo.Payload.Length < REQ_MIN_Length())
                {
                    deviceInfo.ParserStatus = ProtocolParserStatus.InSufficientData;
                    return deviceInfo;
                }

                if (ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4) == "7878")
                {
                    int packetLength = GetNumber(ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2));
                    string packetComparer = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 2);

                    if (packetComparer == ProtocolNumber.LoginMessage)
                    {
                        if (tPayload.Length >= (PayloadLength.LoginMessage * 2))
                        {
                            // Login message
                            deviceInfo.DeviceId = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 16);
                            deviceInfo.CommandType = DeviceCommandType.LoginData;
                            byte[] forCheckSumData = { 0x05,
                                0x01,
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
                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.LoginMessage * 2);
                            log.DebugFormat("{0}/Parse data: {1}", "GT03AProtocol", payloadByte);
                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;

                        }
                    }
                    else if (packetComparer == ProtocolNumber.GPSInformationdata)
                    {
                        if (tPayload.Length >= (PayloadLength.GPSInformation * 2))
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
                            
                            var serialNo = ParserHelper.TSubstring(ref deviceInfo.Payload, 0, 4);

                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.GPSInformation * 2);

                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }
                    }
                    else if (packetComparer == ProtocolNumber.StatusInformation)
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
                    else if (packetComparer == ProtocolNumber.LBSInformationdata)
                    {
                        if (tPayload.Length >= (PayloadLength.LBSInformation * 2))
                        {
                            ParserHelper.TSkip(ref tPayload, ref deviceInfo.Payload, PayloadLength.LBSInformation * 2);

                            deviceInfo = SetDataParsedTime(deviceInfo);
                            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
                        }
                    }
                    else if (packetComparer == ProtocolNumber.StatusInformation)
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

                    log.DebugFormat("Device IP: {0}, payLoad parsed", deviceInfo.TrackerIp);

                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Parse Error: {0}", ex);
            }

            return deviceInfo;






        }

    }

}

























