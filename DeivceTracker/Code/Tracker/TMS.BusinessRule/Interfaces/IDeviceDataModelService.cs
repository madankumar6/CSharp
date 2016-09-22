using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IDeviceDataModelService
    {
        DeviceModels GetDevice(Guid dealerGuid);
        void CreateDevice(DeviceModels device);
        void UpdateDevice(DeviceModels device);
        void SaveDevice();
        CollectionPage<DeviceModels> GetDevices(Guid vehicleGuid, int start, int length);
        void DeleteDevice(Guid deviceId);
        void DeleteDevice(string[] deviceIds);
        void DeleteDevice(DeviceModels device);
        //List<DeviceType> GetDeviceTypes();
    }
}
