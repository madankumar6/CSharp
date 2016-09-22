using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.BusinessRule.Interfaces
{
    public interface IUserService<T> where T : class 
    {
        void CreateUser(T user);
        void DeleteUser(T user);
        void UpdateUser(T user);
        T GetUser(Guid userId);
        T GetUser(string username);
        IEnumerable<T> GetUsers();
        IEnumerable<T> GetUsers(string FirstName);
        T ValidateUser(string username, string password);
        void SaveUser();
    }
}
