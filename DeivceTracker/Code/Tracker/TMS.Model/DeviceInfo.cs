using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public class DeviceInfo
    {
        public Guid DeviceInfoId { get; set; }
        public string DeviceCode { get; set; }
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
        public string UnparsedPayLoad { get; set; }
        public string Distance { get; set; }
        public string Odometer { get; set; }
        public bool OnBattery { get; set; }
        public bool OnIgnition { get; set; }
        public bool OnAc { get; set; }
        public bool OnGps { get; set; }
        public string UnKnown { get; set; }
        public string GeoZoneIndex { get; set; }
        public string GeoZoneId { get; set; }
        public string TrackerIp { get; set; }
        public DateTime DeviceDataTime { get; set; }
        public DateTime TrackerConnectedTime { get; set; }
        public DateTime TrackerDataActionTime { get; set; }
        public DateTime TrackerDataParsedTime { get; set; }
        public DateTime ActionTime { get; set; }

        public Guid DeviceId { get; set; }
        public Device Device { get; set; }

        public DeviceInfo()
        {
            this.DeviceInfoId = Guid.NewGuid();
            this.ActionTime = DateTime.UtcNow;
        }
    }
}
