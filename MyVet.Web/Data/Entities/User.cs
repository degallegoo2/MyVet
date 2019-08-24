using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Address { get; internal set; }
        public string Document { get; internal set; }
    }
}
