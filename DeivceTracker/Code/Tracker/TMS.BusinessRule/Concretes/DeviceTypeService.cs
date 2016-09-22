using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Concretes;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Concretes
{
   public class DeviceTypeService : IDeviceTypeService
    {

        private readonly IDeviceTypeRepository<DeviceType> devicetypeRepository;
        private readonly IUnitOfWork unitofWork;

        public DeviceTypeService(IDeviceTypeRepository<DeviceType> devicetypeRepository,IUnitOfWork _unitofWork)
        {
            this.devicetypeRepository = devicetypeRepository;
            this.unitofWork = _unitofWork;
        }

        public void CreateDeviceType(DeviceType devicetype)
        {
            devicetypeRepository.Add(devicetype);
        }

        public void UpdateDeviceType(DeviceType devicetype)
        {
            devicetypeRepository.Update(devicetype);
        }
        public void SaveDeviceType()
        {
            unitofWork.Commit();
        }

        public DeviceType GetDeviceType(Guid deviceTypeId)
        {
            return devicetypeRepository.GetById(deviceTypeId);
        }

        public CollectionPage<DeviceType> GetDeviceTypes(int currentPage, int itemsPerPage)
        {
            return devicetypeRepository.GetDeviceTypes(currentPage, itemsPerPage); ;
        }

        public void DeleteDeviceType(Guid deviceId)
        {
            devicetypeRepository.Delete(dev => dev.DeviceTypeId == deviceId);
        }

        public void DeleteDeviceType(string[] deviceIds)
        {
            foreach (var customerId in deviceIds)
            {
                Guid userId = Guid.Parse(customerId);
                devicetypeRepository.Delete(cust => cust.DeviceTypeId == userId);
            }
        }
        public void DeleteDeviceType(DeviceType device)
        {
            devicetypeRepository.Delete(device);
        }

        public List<DeviceType> GetDeviceTypes()
        {
            return devicetypeRepository.GetAll().OfType<DeviceType>().ToList();
        }
    }
}
