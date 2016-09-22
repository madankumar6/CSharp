using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.DAL.Repositories.Interfaces
{
   public interface IDeviceModelRepository<T> : IRepository<T> where T : DeviceModels
    {
        T GetVehicleNo(string VehicleNo);
    }
}
