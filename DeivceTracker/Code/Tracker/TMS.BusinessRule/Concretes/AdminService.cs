using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.BusinessRule.Concretes
{
    public class AdminService : UserService<Admin>, IAdminService
    {
        private readonly IAdminRepository adminRepository;
        private readonly IUnitOfWork unitOfWork;

        public AdminService(IAdminRepository adminRepository, IUnitOfWork unitOfWork) : base(adminRepository, unitOfWork)
        {
            this.adminRepository = adminRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateAdmin(Admin distributor)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdmin(Admin admin)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdmin(Admin admin)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdmin(Guid adminId)
        {
            throw new NotImplementedException();
        }

        public void SaveAdmin()
        {
            throw new NotImplementedException();
        }

        public Admin GetAdmin(Guid adminId)
        {
            throw new NotImplementedException();
        }

        public Admin GetAdmin(string adminUsername)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<string, object> GetAdminDashboard(Guid adminId)
        {
            KeyValuePair<string, object> adminDashboardData;


            return new KeyValuePair<string, object>();
        }

        public KeyValuePair<string, object> GetAdminDashboard()
        {
            throw new NotImplementedException();
        }
    }
}
