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
    public class DeviceTypeRepository : Repository<DeviceType>, IDeviceTypeRepository<DeviceType>
    {
        public DeviceTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public CollectionPage<DeviceType> GetDeviceTypes(int page, int itemsPerPage)
        {

            var pageOfResult = new CollectionPage<DeviceType>()
            {
                CurrentPage = page,
                TotalItems = this.DbContext.DeviceTypes.Count(),
                ItemsPerPage = itemsPerPage,
                Items = this.DbContext.DeviceTypes.OrderBy(item => (true)).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }
    }
}
    
