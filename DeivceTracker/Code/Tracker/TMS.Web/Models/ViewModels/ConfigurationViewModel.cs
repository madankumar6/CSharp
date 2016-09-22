using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class ConfigurationViewModel
    {
        public Guid CustomeId { get; set; }
        public CustomerViewModel Customer { get; set; }

        public Guid VehicleId { get; set; }
        public VehicleViewModel Vehicle { get; set; }

        public Guid DeviceId { get; set; }
        public DeviceViewModel Device { get; set; }
    }
    public class DeviceModelViewModel
    {
        public Guid DeviceId { get; set; }
        public string VehicleNo { get; set; }
        // public DeviceType DeviceType { get; set; }
        public string IMEINo { get; set; }
        public string PrimaryMobile{ get; set; }
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
        // public DateTime Transfer_Date { get; set; }
        public int ToExpiry { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid UserId { get; set; }
        // public Customer Customer { get; set; }
        public string DeviceType { get; set; }
       // public virtual DeviceTypeViewModel DeviceType { get; set; }
        public DeviceModelViewModel()
        {
            this.DeviceId = Guid.NewGuid();
            // this.DeviceType = new DeviceType();
            this.EntryDate = DateTime.UtcNow;
            this.ExpiryDate = DateTime.UtcNow.AddDays(ToExpiry);
        } 
    }

    //public class ProtocolServersViewModel
    //{
    //    public string ProtocolServers { get; set; }
    //}
    
}