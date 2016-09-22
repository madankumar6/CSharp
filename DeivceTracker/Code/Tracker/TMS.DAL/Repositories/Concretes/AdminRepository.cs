using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class AdminRepository : UserRepository<Admin>, IAdminRepository
    {
        public AdminRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public KeyValuePair<string, object> GetAdminDashboardData(Guid adminId)
        {
            throw new NotImplementedException();
        }
    }
}
