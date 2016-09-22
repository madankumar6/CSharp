using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public class Vehicle
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
        public virtual Customer Customer { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public Vehicle()
        {
            this.VehicleId = Guid.NewGuid();
        }
    }
}
