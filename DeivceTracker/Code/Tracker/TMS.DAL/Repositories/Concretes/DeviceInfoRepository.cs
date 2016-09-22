using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class DeviceInfoRepository : Repository<DeviceInfo>, IDeviceInfoRepository
    {
        public DeviceInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}
