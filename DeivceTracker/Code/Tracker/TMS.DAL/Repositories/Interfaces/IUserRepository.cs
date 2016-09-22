using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IUserRepository<T> : IRepository<T> where T : class 
    {
        T GetUserByUsername(string username);
        T Getuser(Guid userId);
        bool IsUserExists(string username);
    }
}
