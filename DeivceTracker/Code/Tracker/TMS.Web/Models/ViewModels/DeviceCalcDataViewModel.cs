using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMS.Web.Models.ViewModels
{
    public class DeviceSettingsViewModel
    {
        public string DeviceId { get; set; }
        public string IMEI { get; set; }
        public int Odometer { get; set; }
        public DateTime ActionTime { get; set; }

        public List<SelectListItem> DeviceList { get; set; }
    }
}