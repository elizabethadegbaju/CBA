using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBA.Models
{
    public class ManageUserRolesViewModel
    {
        public CBAUser User { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
    public class UserRolesViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
