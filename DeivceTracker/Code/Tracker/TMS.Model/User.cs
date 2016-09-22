using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public abstract class User
    {
        //User Properties
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string CompanyName { get; set; }
        public bool Status { get; set; }
        public Guid Parent { get; set; }
        public byte[] RowVersion { get; set; }

        public User()
        {
            this.UserId = Guid.NewGuid();
            this.Address = new Address();
        }
    }
}
