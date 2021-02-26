using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBA.Models
{
    public class CBARole: IdentityRole
    {
        public CBARole(string name) : base(name)
        {

        }
        public CBARole()
        {

        }
        public bool IsEnabled { get; set; }
    }
}
