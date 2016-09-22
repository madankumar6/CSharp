using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Common;
using Tracker.Common.Model;
using Tracker.Parser;
using Utils;

namespace Tracker.Protocol
{

    public static class Parser
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion


        public static DeviceInfo Process(Tracker.Parser.Protocol protocolParser, DeviceInfo deviceInfo)
        {
            log.InfoFormat("{0}/Process:", _fileNm);
         
            bool IsSequenceFirst = true;

            log.DebugFormat("{0}/Process: IsSequenceFirst: {1}", _fileNm, IsSequenceFirst);

            Console.WriteLine("{0} >> {1}", deviceInfo.TrackerIp, BitConverter.ToString(deviceInfo.RawData));

            while ((IsSequenceFirst == true) || 
                (deviceInfo.Payload != null && deviceInfo.Payload.Length >= protocolParser.REQ_MIN_Length()))
            {
                log.DebugFormat("{0}/Process: IsSequenceFirst: {1}, deviceInfo.Payload: {2}, REQ_MIN_Length: {3}",
                    _fileNm, IsSequenceFirst, deviceInfo.Payload, protocolParser.REQ_MIN_Length());

                IsSequenceFirst = false;

                deviceInfo.ParserStatus = ProtocolParserStatus.Initialized;

                bool IsDeviceNeedToIdentify = false;
                if (string.IsNullOrWhiteSpace(deviceInfo.DeviceId))
                {
                    // new connection, first request
                    IsDeviceNeedToIdentify = true;
                }

                deviceInfo = protocolParser.Parse(deviceInfo);

                if (IsDeviceNeedToIdentify)
                {
                    deviceInfo = DeviceData.CopyLastDataOfDevice(deviceInfo);
                }

                log.DebugFormat("{0}/Process: deviceInfo.ParserStatus: {1}", _fileNm, deviceInfo.ParserStatus);

                if (deviceInfo.ParserStatus == ProtocolParserStatus.Parsed)
                {
                    if (DeviceData.IsActiveDevice(deviceInfo.DeviceId))
                    {
                        DeviceData.AddActiveDevice(deviceInfo.DeviceId);
                        DeviceData.SaveData(deviceInfo);
                    }
                    else
                    {
                        DeviceData.AddUnknownDevice(deviceInfo.DeviceId);
                    }

                    deviceInfo.ParserStatus = ProtocolParserStatus.Saved;

                    log.DebugFormat("{0}/Process: deviceInfo.ParserStatus: {1}", _fileNm, deviceInfo.ParserStatus);
                    
                    deviceInfo = DeviceData.ClearProcessedData(deviceInfo);
                    deviceInfo.ParserStatus = ProtocolParserStatus.Finished;

                    log.DebugFormat("{0}/Process: deviceInfo.ParserStatus: {1}", _fileNm, deviceInfo.ParserStatus);
                }
                else
                {
                    break;
                }
            }
            log.InfoFormat("{0}/Process: Finished", _fileNm);
            return deviceInfo;
        }
    }
}
