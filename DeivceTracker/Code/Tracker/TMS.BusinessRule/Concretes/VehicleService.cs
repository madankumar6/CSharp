using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Concretes
{
   public class VehicleService : IVehicleService<Vehicle>

    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;

        public VehicleService(IVehicleRepository _vehicleRepository, IUnitOfWork unitOfWork)
        {
            this.vehicleRepository = _vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateVehicle(Vehicle vehicle)
        {
            bool distributorExists = vehicleRepository.IsVehicleExists(vehicle.VehicleNo);

            if (distributorExists)
                throw new InvalidOperationException("Vehicle already exists");

            //string passwordHash = SecurePasswordHasher.Hash(user.Password);
            //user.Password = passwordHash;
            vehicleRepository.Add(vehicle);
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            vehicleRepository.Delete(vehicle);
        }
    
    public void DeleteVehicle(Guid vehicleId)
    {
        vehicleRepository.Delete(cust => cust.VehicleId == vehicleId);
    }

    public void DeleteVehicle(string[] vehicleIds)
    {
        foreach (var distributorId in vehicleIds)
        {
            Guid userId = Guid.Parse(distributorId);
            vehicleRepository.Delete(cust => cust.VehicleId == userId);
        }
    }
        List<Vehicle> IVehicleService<Vehicle>.GetVehicles()
        {
            return vehicleRepository.GetAll().OfType<Vehicle>().ToList();
        } 
        public void UpdateVehicle(Vehicle vehicle)
        {
            vehicleRepository.Update(vehicle);
        }

        public Vehicle GetVehicle(Guid vehicleId)
        {
            return vehicleRepository.GetById(vehicleId);
        }

        //public Vehicle GetVehicle(string vehicleNo)
        //{
        //    return vehicleRepository.GetVehiclesByNumber(vehicleNo);
        //}

        public IEnumerable<Vehicle> GetUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vehicle> GetVehicles(string vehicleNo)
        {
            return vehicleRepository.GetMany(vehicle => vehicle.VehicleNo.StartsWith(vehicleNo));
        }


        public List<Vehicle> GetVehicles(Guid vehicleId)
        {
            return vehicleRepository.GetMany(vehicle => vehicle.CustomerId == vehicleId).ToList();
        }

        public CollectionPage<Vehicle> GetVehicles(Guid CustomerId, int currentPage, int itemsPerPage)
        {
            return vehicleRepository.GetMany(vehicle => vehicle.CustomerId == CustomerId, currentPage, itemsPerPage);
        }

        public CollectionPage<Vehicle> GetVehicles(int currentPage, int itemsPerPage)
        {
            return vehicleRepository.GetMany(currentPage, itemsPerPage);
        }

        //public T ValidateUser(string username, string password)
        //{
        //    User user = userRepository.GetUserByUsername(username);

        //    if (user != null)
        //    {
        //        if (SecurePasswordHasher.Verify(password, user.Password))
        //            return user as T;
        //    }

        //    return null;
        //}

        public void SaveVehicle()
        {
            unitOfWork.Commit();
        }
    }
}
