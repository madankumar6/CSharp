using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IVehicleService<T> where T : class
    {
        void CreateVehicle(Vehicle vehicle);
        Vehicle GetVehicle(Guid vehicleId);
        void SaveVehicle();
        void UpdateVehicle(Vehicle vehicle);

        //Vehicle GetVehicle(int vehicleId);
        List<Vehicle> GetVehicles();
        //  List<Vehicle> GetVehicles(string vehicleNo);
        List<Vehicle> GetVehicles(Guid vehicleId);
        CollectionPage<Vehicle> GetVehicles(int currentPage, int itemsPerPage);
        CollectionPage<Vehicle> GetVehicles(Guid customerId, int currentPage, int itemsPerPage);
        void DeleteVehicle(Guid vehicleId);
        void DeleteVehicle(string[] vehicleIds);
        void DeleteVehicle(Vehicle vehicle);
    }
}
