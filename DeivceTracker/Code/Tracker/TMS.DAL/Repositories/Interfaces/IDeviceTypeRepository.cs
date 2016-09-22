using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IDeviceTypeRepository<T> : IRepository<T> where T : DeviceType
    {
        CollectionPage<DeviceType> GetDeviceTypes(int currentPage, int itemsPerPage);
    }

}
