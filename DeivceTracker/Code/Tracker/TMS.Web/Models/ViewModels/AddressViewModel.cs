using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class AddressViewModel
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}