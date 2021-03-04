using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.Models
{
    public class CBARole: IdentityRole
    {
        public CBARole(string name) : base(name)
        {
            IsEnabled = true;
        }
        public CBARole()
        {
            IsEnabled = true;
        }

        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; }
    }
}
