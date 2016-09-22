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
    public class DistributorService : UserService<Distributor>, IDistributorService
    {
        private readonly IDistributorRepository distributorRepository;
        private readonly IUnitOfWork unitOfWork;

        public DistributorService(IDistributorRepository distributorRepository, IUnitOfWork unitOfWork) : base(distributorRepository, unitOfWork)
        {
            this.distributorRepository = distributorRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateDistributor(Distributor distributor)
        {
            bool distributorExists = distributorRepository.IsUserExists(distributor.Username);

            if (distributorExists)
                throw new InvalidOperationException("Distributor already exists");

            base.CreateUser(distributor);
        }

        public Distributor GetDistributor(int userId)
        {
            Distributor distributor=distributorRepository.GetById((userId)) as Distributor;
            return distributor;
        }

        public List<Distributor> GetDistributors()
        {
            return distributorRepository.GetAll().OfType<Distributor>().ToList();
        }

        public List<Distributor> GetDistributors(string adminUsername)
        {
            var userRepository = distributorRepository as IUserRepository<User>;
            Guid useGuid = userRepository.GetUserByUsername(adminUsername).UserId;
            return distributorRepository.GetMany(distributor => distributor.Parent == useGuid).ToList();
        }

        public List<Distributor> GetDistributors(Guid adminUserId)
        {
            return distributorRepository.GetMany(distributor => distributor.Parent == adminUserId).ToList();
        }

        public CollectionPage<Distributor> GetDistributors(int currentPage, int itemsPerPage)
        {
            return distributorRepository.GetMany(currentPage, itemsPerPage);
        }

        public CollectionPage<Distributor> GetDistributors(Guid adminUserId, int currentPage, int itemsPerPage)
        {
            //return distributorRepository.GetMany(dist => dist.Parent == adminUserId, currentPage, itemsPerPage);
            return distributorRepository.GetDistributorsByAdmin(adminUserId, currentPage, itemsPerPage);
        }

        public void DeleteDistributor(Guid distributorId)
        {
            distributorRepository.Delete(dist => dist.UserId == distributorId);
        }

        public void DeleteDistributor(string[] distributorIds)
        {
            foreach (var distributorId in distributorIds)
            {
                Guid userId = Guid.Parse(distributorId);
                distributorRepository.Delete(dist => dist.UserId == userId);   
            }
        }

        public void DeleteDistributor(Distributor distributor)
        {
            distributorRepository.Delete(distributor);  
        }

        public void SaveDistributor()
        {
            this.unitOfWork.Commit();
        }
    }
}
