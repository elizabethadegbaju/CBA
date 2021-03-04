using CBAData;
using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class UserService : IUserService
    {
        private readonly SignInManager<CBAUser> _signInManager;
        private readonly UserManager<CBAUser> _userManager;
        private readonly RoleManager<CBARole> _roleManager;

        public UserService(SignInManager<CBAUser> signInManager, UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void EditUserRolesAsync(string userId, IList<UserRolesViewModel> userRolesViewModels, CBAUser currentUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, userRolesViewModels.Where(a => a.IsSelected).Select(b => b.Name));
            await _signInManager.RefreshSignInAsync(currentUser);
            await SeedData.SeedSuperUserAsync(_userManager, _roleManager);
        }

        public async Task<ManageUserRolesViewModel> ListUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRolesViewModels = new List<UserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    Name = role.Name,
                    IsSelected = false
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                userRolesViewModels.Add(userRolesViewModel);
            }
            var manageUserRolesViewModel = new ManageUserRolesViewModel()
            {
                User = user,
                UserRoles = userRolesViewModels
            };
            return manageUserRolesViewModel;
        }

        public async Task<List<CBAUser>> ListUsersExceptSpecifiedUserAsync(CBAUser user)
        {
            var allOtherUsers = _userManager.Users.Where(user => user.Id != user.Id);
            return await allOtherUsers.ToListAsync();
        }
    }
}
