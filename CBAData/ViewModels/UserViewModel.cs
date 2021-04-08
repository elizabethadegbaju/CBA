using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Tills = new List<SelectListItem>();
        }
        public string Id;

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; }

        public int TillId { get; set; }
        public string TillAccountNo { get; set; }
        public IList<SelectListItem> Tills { get; set; }

    }
}
