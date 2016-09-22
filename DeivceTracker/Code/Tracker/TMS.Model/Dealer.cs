using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model
{
    public class Dealer : User
    {
        public string ImageName { get; set; }
        public byte[] Logo { get; set; }
    }
}
