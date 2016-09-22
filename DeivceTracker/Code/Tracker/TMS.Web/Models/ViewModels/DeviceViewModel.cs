using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class DeviceViewModel
    {
        public Guid DeviceId { get; set; }
        public string DeviceCode { get; set; }
        public string ModelNo { get; set; }
        public string IMEINo { get; set; }
        public string SIMNo { get; set; }
        public string NetworkProvider { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public bool Status { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }

        public Guid VehicleId { get; set; }
        public VehicleViewModel Vehicle { get; set; }

        public DeviceViewModel()
        {
            this.DeviceId = Guid.NewGuid();
        }
    }
}