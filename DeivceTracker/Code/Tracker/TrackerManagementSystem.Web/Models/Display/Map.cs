using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackerManagementSystem.Web.Models.Display
{
    public class Device
    {
        public string deviceId { get; set; }
        public bool isOnline { get; set; }
    }

    public class Position
    {
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
    }
}