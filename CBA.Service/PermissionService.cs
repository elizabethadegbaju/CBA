using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class PermissionService: IPermissionService
    {
        private readonly RoleManager<CBARole> _roleManager;

        public PermissionService(RoleManager<CBARole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleClaimsViewModel>> ListRoleClaimsAsync(CBARole role)
        {
            var permissions = new List<RoleClaimsViewModel>();
            MemberInfo[] permissionEnum = typeof(Permissions).GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var permission in permissionEnum)
            {
                var name = ((DisplayAttribute)permission.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                permissions.Add(new RoleClaimsViewModel { Value = name, Type = permission.Name });
            }
            var claims = await _roleManager.GetClaimsAsync(role);
            var claimValues = permissions.Select(permission => permission.Type).ToList();
            var roleClaimValues = claims.Select(claim => claim.Type).ToList();
            var authorizedClaims = claimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in permissions)
            {
                if (authorizedClaims.Any(a => a == permission.Type))
                {
                    permission.IsSelected = true;
                }
            }
            return permissions;
        }

        public async void EditRoleClaimsAsync(CBARole role, IEnumerable<RoleClaimsViewModel> roleClaims)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = roleClaims.Where(a => a.IsSelected).ToList();
            foreach (var claim in selectedClaims)
            {
                MemberInfo member = typeof(Permissions).GetMember(claim.Type)[0];
                var name = ((DisplayAttribute)member.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                await _roleManager.AddClaimAsync(role, new Claim(claim.Type, name));
            }
        }
    }
}
