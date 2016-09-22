using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.DAL.Repositories.Concretes
{
    public class DeviceDataModelRepository : Repository<DeviceModels>, IDeviceDataModelRepository
    {
        public DeviceDataModelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

         }
        public CollectionPage<DeviceModels> GetDevicesByCustomer(Guid CustId, int page, int itemsPerPage)
        {

            var pageOfResult = new CollectionPage<DeviceModels>()
            {
                CurrentPage = page,
                TotalItems = this.DbContext.DeviceModels.Count(dist => dist.UserId == CustId),
                ItemsPerPage = itemsPerPage,
                Items = this.DbContext.DeviceModels.Where(dist => dist.UserId == CustId).OrderBy(item => (true)).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }
    }
}
