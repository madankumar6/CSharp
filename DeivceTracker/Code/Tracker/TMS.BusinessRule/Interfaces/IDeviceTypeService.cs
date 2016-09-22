using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IDeviceTypeService
    {
        DeviceType GetDeviceType(Guid dealerGuid);
        void SaveDeviceType();
        void UpdateDeviceType(DeviceType device);
        void CreateDeviceType(DeviceType device);
        CollectionPage<DeviceType> GetDeviceTypes(int start, int length);
        void DeleteDeviceType(Guid deviceId);
        void DeleteDeviceType(string[] deviceIds);
        void DeleteDeviceType(DeviceType device);
        List<DeviceType> GetDeviceTypes();
    }
}
