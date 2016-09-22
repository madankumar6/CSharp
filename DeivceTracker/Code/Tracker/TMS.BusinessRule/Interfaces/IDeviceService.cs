using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IDeviceService <T> where T : Device
    {
        void CreateDevice(Device device);
        Device GetDevice(Guid deviceId);
        void SaveDevice();
         void UpdateDevice(Device device);
        List<Device> GetDevices(Guid deviceId);
        List<Device> GetDevices();
        CollectionPage<Device> GetDevices(int currentPage, int itemsPerPage);
        CollectionPage<Device> GetDevices(Guid deviceId, int currentPage, int itemsPerPage);
        void DeleteDevice(Guid deviceId);
        void DeleteDevice(string[] deviceIds);
        void DeleteDevice(Device device);
    }
}
