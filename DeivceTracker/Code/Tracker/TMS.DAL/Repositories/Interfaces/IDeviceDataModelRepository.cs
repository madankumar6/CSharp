using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IDeviceDataModelRepository : IRepository<DeviceModels>
    {
        CollectionPage<DeviceModels> GetDevicesByCustomer(Guid custId, int currentPage, int itemsPerPage);
    }
}
