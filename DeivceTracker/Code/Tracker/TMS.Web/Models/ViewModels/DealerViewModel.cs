using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.ViewModels
{
    public class DealerViewModel : UserViewModel
    {
        public string ImageName { get; set; }
        public byte[] Logo { get; set; }
    }
}