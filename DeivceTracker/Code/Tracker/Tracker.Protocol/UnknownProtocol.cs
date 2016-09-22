using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Common.Model;

namespace Tracker.Parser
{
    public class UnknownProtocol : Protocol
    {
        const int REQ_MIN_Len = 10;
        const int REQ_MAX_Len = 10;

        public override int REQ_MIN_Length()
        {
            return REQ_MIN_Len;
        }

        public override DeviceInfo Parse(DeviceInfo deviceInfo)
        {
            deviceInfo = SetDataParsedTime(deviceInfo);
            deviceInfo.ParserStatus = ProtocolParserStatus.Parsed;
            return deviceInfo;
        }
    }
}
