using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.BusinessRule.Concretes
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IDeviceInfoRepository deviceInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeviceInfoService(IDeviceInfoRepository deviceInfoRepository, IUnitOfWork unitOfWork)
        {
            this.deviceInfoRepository = deviceInfoRepository;
            this.unitOfWork = unitOfWork;
        }

        public DeviceInfo GetDeviceInfo()
        {
            var deviceInfo = deviceInfoRepository.Get(device => device.CommandType == "2" && device.DeviceInfoId != null);
            return deviceInfo;
        }
    }
}
