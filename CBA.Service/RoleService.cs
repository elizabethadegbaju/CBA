using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<CBAUser> _userManager;
        private readonly RoleManager<CBARole> _roleManager;

        public RoleService(UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckRoleExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }

        public async Task<string> CreateRoleAsync(string name)
        {
            CBARole role;
            await _roleManager.CreateAsync(new CBARole(name));
            role = await _roleManager.FindByNameAsync(name);
            return role.Id;
        }

        public async void EditRoleAsync(string roleId, bool isEnabled, string name)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            role.Name = name;
            role.IsEnabled = isEnabled;
            await _roleManager.UpdateAsync(role);
        }

        public async Task<RoleViewModel> RetrieveRoleDetailAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                IsEnabled = role.IsEnabled
            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roleViewModel.AuthorizedUsers.Add(user);
                    continue;
                }
                roleViewModel.UnAuthorizedUsers.Add(user);
            }
            return roleViewModel;
        }
    }
}
