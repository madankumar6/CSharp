using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public class Customer  : User
    {
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}
