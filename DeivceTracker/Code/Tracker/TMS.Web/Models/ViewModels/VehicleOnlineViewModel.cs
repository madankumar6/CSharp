using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class VehicleOnlineViewModel
    {
        public string DeviceId { get; set; }
        public int Status { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }

        public DeviceData Data { get; set; }
        public class DeviceData
        {
            public string DeviceId { get; set; }
            public string IMEI { get; set; }
            public string CommandType { get; set; }
            public string StatusCode { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Altitude { get; set; }
            public string Speed { get; set; }
            public string Direction { get; set; }
            public string Mileage { get; set; }
            public string ValidData { get; set; }
            public string FullAddress { get; set; }
            public string PayLoad { get; set; }
            public string Distance { get; set; }
            public string Odometer { get; set; }
            public string OnBattery { get; set; }
            public string OnIgnition { get; set; }
            public string OnAc { get; set; }
            public string OnGps { get; set; }
            public string OnAcc { get; set; }
            public string OilNElectricConected { get; set; }
            public string OnSOS { get; set; }
            public string OnLowBattery { get; set; }
            public string OnPowerCut { get; set; }
            public string OnShock { get; set; }
            public string OnCharge { get; set; }
            public string OnDefence { get; set; }
            public string VoltageLevel { get; set; }
            public string SignalStrengthLevel { get; set; }
            public string AlarmType { get; set; }
            public string UnKnown { get; set; }
            public DateTime DeviceDataTime { get; set; }
            public DateTime ActionTime { get; set; }
        }
    }
}