using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;
using TMS.DAL.Repositories.Concretes;

namespace TMS.DAL.Repositories.Interfaces
{
   public interface IVehicleRepository : IRepository<Vehicle>
    {
       // List<Vehicle> GetAllVehicles();
        List<Vehicle> GetVehiclesByNumber(string vehicleNo);
       // Vehicle GetVehicleByVehicleNo(string vehicleNo);
        bool IsVehicleExists(string vehicleNo);
    }
}
