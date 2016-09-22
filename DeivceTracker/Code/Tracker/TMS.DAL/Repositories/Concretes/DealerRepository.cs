using System.Collections.Generic;
using System.Linq;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class DealerRepository : UserRepository<Dealer>, IDealerRepository
    {
        public DealerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }

        public List<Dealer> GetDealersByName(string name)
        {
            //return this.DbContext.Dealers.Where(dealer => dealer.Name == name).ToList();
            return this.DbContext.Users.OfType<Dealer>().Where(dealer => dealer.FirstName == name).ToList();

            //return GetAll().ToList();

            //return GetMany(dealer => dealer.FirstName == name).ToList();
           // return base.GetMany(user => ((Dealer)user).ImageName == "true").OfType<Dealer>().ToList();
        }
    }
}
