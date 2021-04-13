using CBAData.Interfaces;
using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public static async Task SeedRolesAsync(RoleManager<CBARole> roleManager)
        {
            await roleManager.CreateAsync(new CBARole(DefaultRole.SuperUser.ToString()));
        }
        public static async Task SeedSuperUserAsync(UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager)
        {
            var superUser = new CBAUser
            {
                Email = "superuser@appzonegroup.com",
                UserName = "superuser",
                EmailConfirmed = true,
                IsEnabled = true
            };
            if (!userManager.Users.Any(user => user.Id == superUser.Id))
            {
                var user = await userManager.FindByEmailAsync(superUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superUser, "Ade981_");
                    await userManager.AddToRolesAsync(superUser, new List<string>()
                        {
                            DefaultRole.SuperUser.ToString()
                        });
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<CBARole> roleManager)
        {
            var superUserRole = await roleManager.FindByNameAsync("SuperUser");
            var claims = await roleManager.GetClaimsAsync(superUserRole);
            MemberInfo[] permissionEnum = typeof(Permission).GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var permission in permissionEnum)
            {
                var name = ((DisplayAttribute)permission.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                if (!claims.Any(a => a.Type == permission.Name && a.Value == name))
                {
                    await roleManager.AddClaimAsync(superUserRole, new Claim(permission.Name,name));
                }
            }
        }
        public async static Task SeedBankVaultAsync(ApplicationDbContext context)
        {
            if (!context.GLCategories.Any(category => category.Name.ToLower() == "cash assets"))
            {
                var cashAssets = new GLCategory
                {
                    Name = "CASH ASSETS",
                    Type = AccountType.Assets,
                    IsEnabled = true,
                    Description = "GL Category for Cash Assets Accounts"
                };
                context.Add(cashAssets);
                await context.SaveChangesAsync();
            }

            GLCategory category = await context.GLCategories.SingleOrDefaultAsync(c => c.Name.ToLower() == "cash assets");
            if (!context.InternalAccounts.Any(account => account.AccountCode == "10000000000000"))
            {
                var vault = new InternalAccount
                {
                    AccountName = "Vault",
                    AccountBalance = 100000000,
                    AccountCode = "10000000000000",
                    IsActivated = true,
                    GLCategory = category
                };
                context.Add(vault);
                await context.SaveChangesAsync();
            }
        }
    }
}
