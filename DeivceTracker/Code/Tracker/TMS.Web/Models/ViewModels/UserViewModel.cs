using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.Model;
namespace TMS.Web.Models.ViewModels
{

    public class UserViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public AddressViewModel Address { get; set; }
        
        [Required]
        public string PhoneNo { get; set; }
        
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string CompanyName { get; set; }
        public bool Status { get; set; }
        public Guid Parent { get; set; }
        // For handling the concurrency
        public byte[] RowVersion { get; set; }

        public UserViewModel()
        {
            this.UserId = Guid.NewGuid();
            this.Address = new AddressViewModel();
        }
    }
}