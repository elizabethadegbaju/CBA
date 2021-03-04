using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IPermissionService
    {
        Task<List<RoleClaimsViewModel>> ListRoleClaimsAsync(CBARole role);
        Task EditRoleClaimsAsync(CBARole role, IEnumerable<RoleClaimsViewModel> roleClaims);
    }
}
