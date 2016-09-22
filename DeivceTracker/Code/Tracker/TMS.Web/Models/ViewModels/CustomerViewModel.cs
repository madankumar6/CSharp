using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class CustomerViewModel : UserViewModel
    {
        public virtual ICollection<VehicleViewModel> Vehicles { get; set; }
        public virtual ICollection<DeviceViewModel> Devices { get; set; }
    }
}