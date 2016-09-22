using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
   public class DeviceModels
    {
        public Guid DeviceId { get; set; }
        public string VehicleNo { get; set; }
        // public DeviceType DeviceType { get; set; }
        public string IMEINo { get; set; }
        public string PrimaryMobile { get; set; }
        public string SecondaryMobile { get; set; }
        public string Make { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }
        public string Mail { get; set; }
        public string SimNetwork { get; set; }
        public string DeviceSimNo { get; set; }
        public bool Status { get; set; }
        public DateTime EntryDate { get; set; }
        public string TimeZone { get; set; }

        public int ToExpiry { get; set; }
        public DateTime ExpiryDate { get; set; }

        // public DateTime Transfer_Date { get; set; }

        public Guid UserId { get; set; }
        public string DeviceType { get; set; }
        //   public virtual DeviceType DeviceType { get; set; }

        public DeviceModels()
        {
            this.DeviceId = Guid.NewGuid();
            //this.DeviceTypes = new DeviceType();
            this.EntryDate = DateTime.UtcNow;
            this.ExpiryDate = DateTime.UtcNow;
        }
    }
}
