using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IDistributorService : IUserService<Distributor>
    {
        void CreateDistributor(Distributor distributor);
        void SaveDistributor();
        Distributor GetDistributor(int userId);
        List<Distributor> GetDistributors();
        List<Distributor> GetDistributors(string adminUsername);
        List<Distributor> GetDistributors(Guid adminUserId);
        CollectionPage<Distributor> GetDistributors(int currentPage, int itemsPerPage);
        CollectionPage<Distributor> GetDistributors(Guid adminUserId, int currentPage, int itemsPerPage);
        void DeleteDistributor(Guid distributorId);
        void DeleteDistributor(string[] distributorIds);
        void DeleteDistributor(Distributor distributor);

    }
}
