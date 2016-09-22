using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IDealerService : IUserService<Dealer>
    {
        void CreateDealer(Dealer dealer);
        Dealer GetDealer(Guid dealerId);
        Dealer GetDealer(string dealerCode);
        List<Dealer> GetDealers();
        List<Dealer> GetDealers(string distributorCode);
        List<Dealer> GetDealers(Guid distributorUserId);
        CollectionPage<Dealer> GetDealers(int currentPage, int itemsPerPage);
        CollectionPage<Dealer> GetDealers(Guid distributorUserId, int currentPage, int itemsPerPage);
        void DeleteDealer(Guid dealerId);
        void DeleteDealer(string[] dealerIds);
        void DeleteDealer(Dealer dealer);
        void SaveDealer();
    }
}
