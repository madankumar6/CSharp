using System;
using System.Collections.Generic;
using System.Linq;
using TMS.BusinessRule.Interfaces;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Concretes
{
    public class DealerService : UserService<Dealer>, IDealerService
    {
        private readonly IDealerRepository dealerRepository;
        private readonly IUnitOfWork unitOfWork;

        public DealerService(IDealerRepository dealerRepository, IUnitOfWork unitOfWork) : base(dealerRepository, unitOfWork)
        {
            this.dealerRepository = dealerRepository;
            this.unitOfWork = unitOfWork;
        }

        public void CreateDealer(Dealer dealer)
        {
            bool dealerExists = dealerRepository.IsUserExists(dealer.Username);

            if (dealerExists)
                throw new InvalidOperationException("Dealer already exists");

            base.CreateUser(dealer);
        }

        public void DeleteDealer(Guid dealerId)
        {
            dealerRepository.Delete(dist => dist.UserId == dealerId);
        }

        public void DeleteDealer(string[] dealerIds)
        {
            foreach (var distributorId in dealerIds)
            {
                Guid userId = Guid.Parse(distributorId);
                dealerRepository.Delete(dist => dist.UserId == userId);
            }
        }

        public void DeleteDealer(Dealer dealer)
        {
            dealerRepository.Delete(dealer);
        }

        public Dealer GetDealer(Guid dealerId)
        {
            return dealerRepository.Get(dealer => dealer.UserId == dealerId);
        }

        public Dealer GetDealer(string dealerCode)
        {
            return dealerRepository.Get(dealer => dealer.Username == dealerCode);
        }

        List<Dealer> IDealerService.GetDealers()
        {
            return dealerRepository.GetAll().OfType<Dealer>().ToList();
        }

        public Dealer GetDealer(int dealerId)
        {
            Dealer dealer = dealerRepository.GetById(dealerId) as Dealer;
            return dealer;
        }

        public IEnumerable<Dealer> GetDealers()
        {
            return dealerRepository.GetAll().OfType<Dealer>();
        }

        public List<Dealer> GetDealers(Guid distributorId)
        {
            return dealerRepository.GetMany(dealer => dealer.Parent == distributorId).ToList();
        }

        public CollectionPage<Dealer> GetDealers(int currentPage, int itemsPerPage)
        {
            return dealerRepository.GetMany(currentPage, itemsPerPage);
        }

        public CollectionPage<Dealer> GetDealers(Guid distributorId, int currentPage, int itemsPerPage)
        {
            return dealerRepository.GetMany(dealer => dealer.Parent == distributorId, currentPage, itemsPerPage);
        }

        public void SaveDealer()
        {
            this.unitOfWork.Commit();
        }

        public List<Dealer> GetDealers(string adminUsername)
        {
            throw new NotImplementedException();
        }

    }
}
