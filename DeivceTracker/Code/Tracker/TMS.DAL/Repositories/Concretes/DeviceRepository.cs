using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
           
        }

        public List<Device> GetDevicesBycode(string deviceCode)
        {
            return
                this.DbContext.Devices.OfType<Device>()
                    .Where(device => device.DeviceCode == deviceCode)
                    .ToList();
        }

        public bool IsDeviceExists(string DeviceCode)
        {
            return this.DbContext.Devices.OfType<Device>().Any(user => user.DeviceCode == DeviceCode);
        }
    }
}
