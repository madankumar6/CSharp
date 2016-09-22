using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Rules
{
    public class CustomPrincipalSerializeModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public Type UserType { get; set; }
    }
}