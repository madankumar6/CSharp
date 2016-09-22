using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class DeviceTypeViewModel
    {

        public Guid DeviceTypeId { get; set; }
        public string DeviceModel_Type { get; set; }
        public bool Status { get; set; }
        public DateTime Entry_date { get; set; }

        public DeviceTypeViewModel()
        {
            this.DeviceTypeId = Guid.NewGuid();
            this.Entry_date = DateTime.UtcNow;
        }
    }
}