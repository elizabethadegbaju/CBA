using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CBAData
{
    public static class SeedData
    {
        public static async Task SeedRolesAsync(UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager)
        {
            await roleManager.CreateAsync(new CBARole(DefaultRoles.SuperUser.ToString()));
            await roleManager.CreateAsync(new CBARole(DefaultRoles.Admin.ToString()));
            await roleManager.CreateAsync(new CBARole(DefaultRoles.Staff.ToString()));
            await roleManager.CreateAsync(new CBARole(DefaultRoles.Customer.ToString()));
        }
        public static async Task SeedSuperUserAsync(UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager)
        {
            var superUser = new CBAUser
            {
                Email = "superuser@appzonegroup.com",
                UserName = "superuser",
                EmailConfirmed = true,
            };
            if (!userManager.Users.Any(user => user.Id == superUser.Id))
            {
                var user = await userManager.FindByEmailAsync(superUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superUser, "Ade981_");
                    await userManager.AddToRolesAsync(superUser, new List<string>()
                        {
                            DefaultRoles.Admin.ToString(),
                            DefaultRoles.SuperUser.ToString(),
                            DefaultRoles.Staff.ToString(),
                            DefaultRoles.Customer.ToString()
                        });
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<CBARole> roleManager)
        {
            var superUserRole = await roleManager.FindByNameAsync("SuperUser");
            var claims = await roleManager.GetClaimsAsync(superUserRole);
            MemberInfo[] permissionEnum = typeof(Permissions).GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var permission in permissionEnum)
            {
                var name = ((DisplayAttribute)permission.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                if (!claims.Any(a => a.Type == permission.ToString() && a.Value == name))
                {
                    await roleManager.AddClaimAsync(superUserRole, new Claim(permission.ToString(),name));
                }
            }
        }
    }
}
