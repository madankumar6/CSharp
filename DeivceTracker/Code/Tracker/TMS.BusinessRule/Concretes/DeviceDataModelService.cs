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
    public class DeviceDataModelService : IDeviceDataModelService
    {
        private readonly IDeviceDataModelRepository devicemodelRepository;
       // private readonly IDeviceTypeRepository<DeviceType> devicetypeRepository;
        private readonly IUnitOfWork _unitofwork;

        public DeviceDataModelService(IDeviceDataModelRepository devicemodelRepository, IUnitOfWork _unitofwork)
        {
            this.devicemodelRepository = devicemodelRepository;
            this._unitofwork = _unitofwork;
           // this.devicetypeRepository = devicetypeRepository;
        }

        public DeviceModels GetDevice(Guid DeviceId)
        {
            return devicemodelRepository.GetById(DeviceId);
        }

        public void CreateDevice(DeviceModels device)
        {
            devicemodelRepository.Add(device);
        }

        public void UpdateDevice(DeviceModels device)
        {
            devicemodelRepository.Update(device);
        }

        public CollectionPage<DeviceModels> GetDevices(Guid CustId, int currentPage, int itemsPerPage)
        {
            return devicemodelRepository.GetDevicesByCustomer(CustId, currentPage, itemsPerPage); ;
        }

        public void DeleteDevice(Guid deviceId)
        {
            devicemodelRepository.Delete(dev => dev.DeviceId == deviceId);
        }

        public void DeleteDevice(string[] deviceIds)
        {
            foreach (var customerId in deviceIds)
            {
                Guid userId = Guid.Parse(customerId);
                devicemodelRepository.Delete(cust => cust.DeviceId == userId);
            }
        }

        public void DeleteDevice(DeviceModels device)
        {
            devicemodelRepository.Delete(device);
        }

        //public List<DeviceType> GetDeviceTypes()
        //{
        //    return devicetypeRepository.GetAll().OfType<DeviceType>().ToList();
        //}

        public void SaveDevice()
        {
            this._unitofwork.Commit();
        }
    }
}
