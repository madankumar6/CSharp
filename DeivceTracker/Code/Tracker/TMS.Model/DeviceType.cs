using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
   public class DeviceType
    {
        public Guid DeviceTypeId { get; set; }
        public string DeviceModel_Type { get; set; }
        public bool Status { get; set; }
        public DateTime EntryDate { get; set; }

        public DeviceType()
        {
            this.DeviceTypeId = Guid.NewGuid();
            this.EntryDate = DateTime.UtcNow;
        }
    }
}
