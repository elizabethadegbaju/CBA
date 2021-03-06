using CBAData.Interfaces;
using CBAData.Models;
using CBAData.ViewModels;
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
            await _roleManager.CreateAsync(new CBARole(name));
            CBARole role;
            role = await _roleManager.FindByNameAsync(name);
            return role.Id;
        }

        public async Task EditRoleAsync(string roleId, bool isEnabled, string name)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            role.Name = name;
            role.IsEnabled = isEnabled;
            await _roleManager.UpdateAsync(role);
        }

        public async Task ManageRoleUsers(string id, IList<RoleUsersViewModel> roleUsersViewModel)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var authorizedUsers = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in authorizedUsers)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            foreach (var roleUser in roleUsersViewModel)
            {
                if (roleUser.IsAuthorized)
                {
                    var user = await _userManager.FindByIdAsync(roleUser.User.Id);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
        }

        public async Task<ManageRoleUsersViewModel> RetrieveRoleDetailAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var manageRoleUsersViewModel = new ManageRoleUsersViewModel
            {
                Role = role,
                RoleUsers = new List<RoleUsersViewModel>()
            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageRoleUsersViewModel.RoleUsers.Add(new RoleUsersViewModel
                    {
                        User = user,
                        IsAuthorized = true
                    });
                    continue;
                }
                manageRoleUsersViewModel.RoleUsers.Add(new RoleUsersViewModel
                {
                    User = user,
                    IsAuthorized = false
                });
            }
            return manageRoleUsersViewModel;
        }
    }
}
