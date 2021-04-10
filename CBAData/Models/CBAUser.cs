using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.Models
{
    public class CBAUser : IdentityUser
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; }

        public string CBARoleId { get; set; }

        [DisplayName("Role")]
        public CBARole CBARole { get; set; }
        
        public InternalAccount Till { get; set; }

        public List<Posting> Postings { get; set; }
    }
}
