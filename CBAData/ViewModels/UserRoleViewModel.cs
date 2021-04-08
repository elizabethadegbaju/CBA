using CBAData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.ViewModels
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            Roles = new List<SelectListItem>();
        }
        public CBAUser User { get; set; }
        public string RoleId { get; set; }
        public IList<SelectListItem> Roles { get; set; }
    }
}
