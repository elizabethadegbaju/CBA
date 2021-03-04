using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.Models
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            AuthorizedUsers = new List<CBAUser>();
            UnAuthorizedUsers = new List<CBAUser>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; }

        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; }

        public List<CBAUser> AuthorizedUsers { get; set; }

        public List<CBAUser> UnAuthorizedUsers { get; set; }
    }
}
