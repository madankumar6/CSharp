using System;
using System.Collections.Generic;
using System.Linq;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.DAL.Repositories.Concretes
{
    public class DistributorRepository : UserRepository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public List<Distributor> GetDistributorsByName(string name)
        {
            return
                this.DbContext.Distributors.OfType<Distributor>()
                    .Where(distributor => distributor.FirstName == name)
                    .ToList();
        }

        public CollectionPage<Distributor> GetDistributorsByAdmin(Guid adminId, int page, int itemsPerPage)
        {

            var pageOfResult = new CollectionPage<Distributor>()
            {
                CurrentPage = page,
                TotalItems = this.DbContext.Distributors.Count(dist => dist.Parent == adminId),
                ItemsPerPage = itemsPerPage,
                Items = this.DbContext.Distributors.Where(dist => dist.Parent == adminId).OrderBy(item => (true)).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }
    }
}
