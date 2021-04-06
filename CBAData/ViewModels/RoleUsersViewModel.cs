using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBAData.ViewModels
{
    public class ManageRoleUsersViewModel
    {
        public CBARole Role { get; set; }
        public IList<RoleUsersViewModel> RoleUsers { get; set; }
    }
    public class RoleUsersViewModel
    {
        public CBAUser User { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
