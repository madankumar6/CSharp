using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class Position
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class DeviceDataViewModel
    {
        public string DeviceId { get; set; }
        public string IMEI { get; set; }
        public string StatusCode { get; set; }
        public Position Position { get; set; }
        public string Speed { get; set; }
        public string Direction { get; set; }
        public string Altitude { get; set; }
        public string Mileage { get; set; }
        public string ValidData { get; set; }
        public string FullAddress { get; set; }
        public string Payload { get; set; }
        public string UnParsedPayload { get; set; }
        public string Distance { get; set; }
        public string Odometer { get; set; }
        public int? OnBattery { get; set; }
        public int? OnIgnition { get; set; }
        public int? OnAc { get; set; }
        public int? OnGps { get; set; }
        public string UnKnown { get; set; }
        public DateTime? DeviceDataTime { get; set; }
        public string GeozoneIndex { get; set; }
        public string GeozoneID { get; set; }
        public string TrackerIp { get; set; }

        public DateTime? TrackerConnectedTime { get; set; }
        public DateTime? TrackerDataActionTime { get; set; }
        public DateTime? TrackerDataParsedTime { get; set; }

        public DateTime ActionTime { get; set; }
    }

}