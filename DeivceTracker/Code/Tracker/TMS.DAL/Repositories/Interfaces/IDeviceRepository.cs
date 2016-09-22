﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IDeviceRepository : IRepository<Device>
    {
        List<Device> GetDevicesBycode(string deviceCode);
        bool IsDeviceExists(string DeviceCode);
    }
}
