using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ApplicationUser: IdentityUser
    {
        //Made our own user because the regular identity user has no property for name.
        public string Name { get; set; }
    }
}
