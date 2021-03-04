using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public interface IPermissionService
    {
        Task<List<RoleClaimsViewModel>> ListRoleClaimsAsync(CBARole role);
        void EditRoleClaimsAsync(CBARole role, IEnumerable<RoleClaimsViewModel> roleClaims);
    }
}
