using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;

namespace TMS.DAL.Repositories.Concretes
{
    public class CustomerRepository : UserRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public List<Customer> GetCustomersByName(string name)
        {
            return
                this.DbContext.Customers.OfType<Customer>()
                    .Where(customer => customer.FirstName == name)
                    .ToList();
        }
    }
}
