using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IDistributorRepository : IUserRepository<Distributor>
    {
       // List<Distributor> GetDistrbutorsByName(string name); 
        List<Distributor> GetDistributorsByName(string name);
        CollectionPage<Distributor> GetDistributorsByAdmin(Guid adminId, int page, int itemsPerPage);
    }


}
