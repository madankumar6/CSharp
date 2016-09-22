using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public class Device
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
        public Customer Customer { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public Device()
        {
            this.DeviceId = Guid.NewGuid();
        }
    }
}
