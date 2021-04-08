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
        Task<UserRoleViewModel> ViewUserRoleAsync(string userId);

        Task EditUserRoleAsync(UserRoleViewModel userRoleViewModel);

        Task<List<CBAUser>> ListUsersAsync();

        UserRoleViewModel GetCreateUserAsync();

        Task<CBAUser> CreateUserAsync(CBAUser modelUser, string password);

        Task UpdateUserRoleAsync(CBAUser user, string role);

        void SendAccountConfirmationEmail(string pathToFile, string callbackUrl, CBAUser user, string password);

        void SendPasswordResetEmail(string pathToFile, string callbackUrl, CBAUser user);

        void SendEmailFromTemplate(CBAUser user, string subject, MimeEntity content);

        Task DeleteUserAsync(string userId);

        Task EditUserAsync(string id, UserViewModel userViewModel);
        Task<UserViewModel> GetEditUserAsync(string id);

        Task<List<GLAccount>> FetchAvailableTills();
        
        Task AssignTill(int tillId, CBAUser user);
        
        Task UnAssignTill(string userId);
    }
}
