using CBAData.Models;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{ 
    public interface IUserService
    {
        Task<ManageUserRolesViewModel> ListUserRolesAsync(string userId);

        Task EditUserRolesAsync(string userId, IList<UserRolesViewModel> userRolesViewModels, CBAUser currentUser);

        Task<List<CBAUser>> ListUsersAsync();

        ManageUserRolesViewModel LoadEmptyUser();

        Task<CBAUser> CreateUserAsync(CBAUser modelUser, string password);
        Task<UserViewModel> GetCreateUserAsync();

        Task UpdateUserRolesAsync(CBAUser user, List<string> userRoles);

        void SendAccountConfirmationEmail(string pathToFile, string callbackUrl, CBAUser user, string password);

        void SendPasswordResetEmail(string pathToFile, string callbackUrl, CBAUser user);

        void SendEmailFromTemplate(CBAUser user, string subject, MimeEntity content);

        Task DeleteUserAsync(string userId);

        Task EditUserAsync(string id, UserViewModel userViewModel);
        Task<UserViewModel> GetEditUserAsync(string id);

        Task<List<GLAccount>> FetchTills();
        
        Task AssignTill(int tillId, CBAUser user);
        
        Task UnAssignTill(int tillId);
    }
}
