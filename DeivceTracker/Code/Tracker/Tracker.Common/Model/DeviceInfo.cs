using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.Common.Model
{
    public class DeviceInfo
    {
        #region DBVAlues
        public string DeviceId { get; set; }
        public string IMEI { get; set; }
        public DeviceCommandType CommandType { get; set; }
        
        public string StatusCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Speed { get; set; }
        public string Direction { get; set; }
        public string Altitude { get; set; }
        public string Mileage { get; set; }
        public string ValidData { get; set; }
        public string FullAddress { get; set; }
        public string Payload;
        public string UnParsedPayload { get; set; }
        public string Distance { get; set; }
        public int? Odometer { get; set; }
        public int? OnIgnition { get; set; }
        public int? OnAc { get; set; }

        public int? OnAcc { get; set; }
        public int? OilNElectricConected { get; set; }
        public int? OnSOS { get; set; }
        public int? OnLowBattery { get; set; }
        public int? OnPowerCut { get; set; }
        public int? OnShock { get; set; }
        public int? OnCharge { get; set; }
        public int? OnDefence { get; set; }
        public int? VoltageLevel { get; set; }
        public int? SignalStrengthLevel { get; set; }
        public DeviceAlarmType AlarmType { get; set; }

        public int? OnGps { get; set; }
        public string UnKnown { get; set; }
        public DateTime? DeviceDataTime;
        public string GeozoneIndex { get; set; }
        public string GeozoneID { get; set; }
        public string TrackerIp { get; set; }
        public DateTime? TrackerConnectedTime { get; set; }
        public DateTime? TrackerDataActionTime { get; set; }
        public DateTime? TrackerDataParsedTime { get; set; }
        #endregion

        public string ReplyMessage { get; set; }

        public byte[] RawData { get; set; }
        public byte[] ToSendRawData { get; set; }

        public DateTime ActionTime { get; set; }

        public ProtocolParserStatus ParserStatus { get; set; }

        public DeviceInfo()
        {
            ParserStatus = ProtocolParserStatus.Initialized;
        }
    }
}
