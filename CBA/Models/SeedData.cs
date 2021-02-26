using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CBA.Models
{
    public static class SeedData
    {
        public static async Task SeedRolesAsync(UserManager<CBAUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Role.SuperUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Staff.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Customer.ToString()));
        }
        public static async Task SeedSuperUserAsync(UserManager<CBAUser> userManager, RoleManager<IdentityRole> roleManager)
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
                            Role.Admin.ToString(),
                            Role.SuperUser.ToString(),
                            Role.Staff.ToString(),
                            Role.Customer.ToString()
                        });
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
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
