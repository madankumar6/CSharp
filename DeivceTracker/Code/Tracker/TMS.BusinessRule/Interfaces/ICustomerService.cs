using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface ICustomerService : IUserService<Customer>
    {
        void CreateCustomer(Customer customer);
        Customer GetCustomer(Guid dealerId);
        void SaveCustomer();
        Customer GetCustomer(int userId);
        List<Customer> GetCustomers();
        List<Customer> GetCustomers(string dealerCode);
        List<Customer> GetCustomers(Guid delaerId);
        CollectionPage<Customer> GetCustomers(int currentPage, int itemsPerPage);
        CollectionPage<Customer> GetCustomers(Guid delaerId, int currentPage, int itemsPerPage);
        void DeleteCustomer(Guid customerId);
        void DeleteCustomer(string[] customerIds);
        void DeleteCustomer(Customer customer);

    }
}
