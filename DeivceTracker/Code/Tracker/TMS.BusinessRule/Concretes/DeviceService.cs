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
   public class DeviceService : IDeviceService<Device>
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeviceService(IDeviceRepository _deviceRepository, IUnitOfWork unitOfWork)
        {
            this.deviceRepository = _deviceRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateDevice(Device device)
        {
            bool diviceExists = deviceRepository.IsDeviceExists(device.DeviceCode);

            if (diviceExists)
                throw new InvalidOperationException("Device already exists");

            deviceRepository.Add(device);
        }

        public void UpdateDevice(Device device)
        {
            deviceRepository.Update(device);
        }

        public Device GetDevice(int deviceId)
        {
            Device Device = deviceRepository.GetById((deviceId)) as Device;
            return Device;
        }
        public Device GetDevice(Guid deviceId)
        {
            return deviceRepository.Get(device => device.DeviceId == deviceId);
        }
        public List<Device> GetDevices()
        {
            return deviceRepository.GetAll().OfType<Device>().ToList();
        }

        public List<Device> GetDevices(Guid customerId)
        {
            return deviceRepository.GetMany(device => device.CustomerId == customerId).ToList();
        }

        List<Device> IDeviceService<Device>.GetDevices()
        {
            return deviceRepository.GetAll().OfType<Device>().ToList();
        }
        public CollectionPage<Device> GetDevices(int currentPage, int itemsPerPage)
        {
            return deviceRepository.GetMany(currentPage, itemsPerPage);
        }

        public CollectionPage<Device> GetDevices(Guid deviceId, int currentPage, int itemsPerPage)
        {
            return deviceRepository.GetMany(dev => dev.CustomerId == deviceId, currentPage, itemsPerPage);
        }
        public void DeleteDevice(Guid deviceId)
        {
            deviceRepository.Delete(dev => dev.DeviceId == deviceId);
        }

        public void DeleteDevice(string[] deviceIds)
        {
            foreach (var customerId in deviceIds)
            {
                Guid userId = Guid.Parse(customerId);
                deviceRepository.Delete(cust => cust.CustomerId == userId);
            }
        }

        public void DeleteDevice(Device Device)
        {
            deviceRepository.Delete(Device);
        }
        public void SaveDevice()
        {
            this.unitOfWork.Commit();
        }

    }
}
