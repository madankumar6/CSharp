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
    public class CustomerService : UserService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : base(customerRepository, unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
        }

       public void CreateCustomer(Customer customer)
        {
            bool CustomerExists = customerRepository.IsUserExists(customer.Username);

            if (CustomerExists)
                throw new InvalidOperationException("Customer already exists");

            customerRepository.Add(customer);
        }

        public Customer GetCustomer(int userId)
        {
            Customer customer = customerRepository.GetById((userId)) as Customer;
            return customer;
        }
        public Customer GetCustomer(Guid customerId)
        {
            return customerRepository.Get(customer => customer.UserId == customerId);
        }
        public List<Customer> GetCustomers()
        {
            return customerRepository.GetAll().OfType<Customer>().ToList();
        }

        public List<Customer> GetCustomers(string CustomerCode)
        {
            var userRepository = customerRepository as IUserRepository<User>;
            Guid useGuid = userRepository.GetUserByUsername(CustomerCode).UserId;
            return customerRepository.GetMany(customer => customer.Parent == useGuid).ToList();
        }

        public List<Customer> GetCustomers(Guid delaerId)
        {
            return customerRepository.GetMany(customer => customer.Parent == delaerId).ToList();
        }

        public CollectionPage<Customer> GetCustomers(int currentPage, int itemsPerPage)
        {
            return customerRepository.GetMany(currentPage, itemsPerPage);
        }

        public CollectionPage<Customer> GetCustomers(Guid CustomerId, int currentPage, int itemsPerPage)
        {
            return customerRepository.GetMany(cust => cust.Parent == CustomerId, currentPage, itemsPerPage);
        }
        public void DeleteCustomer(Guid customerId)
        {
            customerRepository.Delete(cust => cust.UserId == customerId);
        }

        public void DeleteCustomer(string[] customerIds)
        {
            foreach (var distributorId in customerIds)
            {
                Guid userId = Guid.Parse(distributorId);
                customerRepository.Delete(cust => cust.UserId == userId);
            }
        }

        public void DeleteCustomer(Customer customer)
        {
            customerRepository.Delete(customer);
        }
        public void SaveCustomer()
        {
            this.unitOfWork.Commit();
        }
    }
}
