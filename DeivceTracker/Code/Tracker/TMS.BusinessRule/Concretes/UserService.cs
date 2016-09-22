using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Concretes
{
    public class UserService<T> : IUserService<T> where T : User
    {
        private readonly IUserRepository<T> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository<T> userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateUser(T user)
        {
            string passwordHash = SecurePasswordHasher.Hash(user.Password);
            user.Password = passwordHash;
            userRepository.Add(user);
        }

        public void DeleteUser(T user)
        {
            userRepository.Delete(user);
        }

        public void UpdateUser(T user)
        {
            userRepository.Update(user);
        }

        public T GetUser(Guid userId)
        {
            return userRepository.GetById(userId);
        }

        public T GetUser(string username)
        {
            return userRepository.GetUserByUsername(username);
        }

        public IEnumerable<T> GetUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetUsers(string firstName)
        {
            return userRepository.GetMany(user => user.FirstName.StartsWith(firstName));
        }

        public T ValidateUser(string username, string password)
        {
            User user = userRepository.GetUserByUsername(username);
        
            if (user != null)
            {
                if (SecurePasswordHasher.Verify(password, user.Password))
                    return user as T;
            }

            return null;
        }

        public void SaveUser()
        {
            unitOfWork.Commit();
        }
    }
}
