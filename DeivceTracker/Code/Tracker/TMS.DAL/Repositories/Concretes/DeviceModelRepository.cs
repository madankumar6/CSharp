using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class DeviceModelRepository : Repository<DeviceModels>, IDeviceModelRepository<DeviceModels>
    {
        public DeviceModelRepository(DbFactory dbfactory) : base(dbfactory)
        {

        }

        public DeviceModels GetVehicleNo(string VehicleNo)
        {
            return Get(m => m.VehicleNo == VehicleNo);
        }
    }
}
