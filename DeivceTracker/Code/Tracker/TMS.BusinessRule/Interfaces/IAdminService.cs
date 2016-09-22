using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.BusinessRule.Interfaces
{
    public interface IAdminService : IUserService<Admin>
    {
        void CreateAdmin(Admin distributor);
        void UpdateAdmin(Admin admin);
        void DeleteAdmin(Admin admin);
        void DeleteAdmin(Guid adminId);
        void SaveAdmin();
        Admin GetAdmin(Guid adminId);
        Admin GetAdmin(string adminUsername);
        KeyValuePair<string, object> GetAdminDashboard(Guid adminId);

    }
}
