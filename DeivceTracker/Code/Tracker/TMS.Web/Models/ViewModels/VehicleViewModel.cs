using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class AvailableDeviceViewModel
    {
        public string DeviceId { get; set; }
		public string IMEINo{ get; set; }

        public string VehicleNo{ get; set; }
		//public string PrimaryMobile{ get; set; }
		//public string SecondaryMobile{ get; set; }
		public string Make{ get; set; }
		public string VehicleModel{ get; set; }
		public string VehicleType{ get; set; }
		//public string Mail{ get; set; }
		public string SimNetwork{ get; set; }
		public string DeviceSimNo{ get; set; }
		public string TimeZone{ get; set; }
        public string EntryDate{ get; set; }
		public string ExpiryDate{ get; set; }
		public string DeviceType{ get; set; }
    }

    public class VehicleViewModel
    {
        public Guid VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Type { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public bool Status { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }

        public VehicleViewModel()
            {
            this.VehicleId = Guid.NewGuid();
            }
    }
}