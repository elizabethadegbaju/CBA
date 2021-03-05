using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{ 
    public interface IUserService
    {
        Task<ManageUserRolesViewModel> ListUserRolesAsync(string userId);

        void EditUserRolesAsync(string userId, IList<UserRolesViewModel> userRolesViewModels, CBAUser currentUser);

        Task<List<CBAUser>> ListUsersExceptSpecifiedUserAsync(CBAUser user);

        ManageUserRolesViewModel LoadEmptyUser();

        Task<CBAUser> CreateUserAsync(CBAUser modelUser, string password);

        Task UpdateUserRolesAsync(CBAUser user, List<string> userRoles);

        void SendAccountConfirmationEmail(string pathToFile, string callbackUrl, CBAUser user, string password);

        Task DeleteUserAsync(string userId);

        Task UpdateUserAsync(string id, CBAUser user);
    }
}
