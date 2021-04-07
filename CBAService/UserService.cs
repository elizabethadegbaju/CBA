using MailKit.Net.Smtp;
using CBAData;
using CBAData.Interfaces;
using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CBAService
{
    public class UserService : IUserService
    {
        private readonly SignInManager<CBAUser> _signInManager;
        private readonly UserManager<CBAUser> _userManager;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly EmailMetadata _emailMetadata;
        private readonly ApplicationDbContext _context;

        public UserService(SignInManager<CBAUser> signInManager, UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager, EmailMetadata emailMetadata, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailMetadata = emailMetadata;
            _context = context;
        }

        public async Task AssignTill(int tillId, CBAUser user)
        {
            var till = await _context.GLAccounts.FindAsync(tillId);
            till.User = user;
            _context.Update(till);
            await _context.SaveChangesAsync();
        }

        public async Task<CBAUser> CreateUserAsync(CBAUser modelUser, string password)
        {
            MailAddress address = new MailAddress(modelUser.Email);
            string userName = address.User;
            var user = new CBAUser { 
                UserName = userName, 
                Email = modelUser.Email, 
                FirstName = modelUser.FirstName, 
                LastName = modelUser.LastName
            };
            await _userManager.CreateAsync(user, password);
            return await _userManager.FindByIdAsync(user.Id);
        }
        
        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }

        public async Task EditUserRolesAsync(string userId, IList<UserRolesViewModel> userRolesViewModels, CBAUser currentUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, userRolesViewModels.Where(a => a.IsSelected).Select(b => b.Name));
            await _signInManager.RefreshSignInAsync(currentUser);
            await SeedData.SeedSuperUserAsync(_userManager, _roleManager);
        }

        public async Task<List<GLAccount>> FetchTills()
        {
            var cashAssetsCategory = _context.GLCategories.Where(c => (c.Type == AccountType.Assets) & (c.Name.ToLower() == "cash assets")).FirstOrDefault();
            return await _context.GLAccounts.Where(t => t.GLCategory == cashAssetsCategory).ToListAsync();
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

        public async Task<List<CBAUser>> ListUsersAsync()
        {
            var allOtherUsers = await _userManager.Users.ToListAsync();
            return allOtherUsers;
        }

        public ManageUserRolesViewModel LoadEmptyUser()
        {
            var userRolesViewModels = new List<UserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    Name = role.Name,
                    IsSelected = false
                };
                userRolesViewModels.Add(userRolesViewModel);
            }
            var manageUserRolesViewModel = new ManageUserRolesViewModel()
            {
                User = new CBAUser(),
                UserRoles = userRolesViewModels
            };
            return manageUserRolesViewModel;
        }

        public void SendAccountConfirmationEmail(string pathToFile, string callbackUrl, CBAUser user, string password)
        {
            var builder = new BodyBuilder();
            using StreamReader SourceReader = File.OpenText(pathToFile);
            builder.HtmlBody = SourceReader.ReadToEnd();
            builder.HtmlBody = string.Format(builder.HtmlBody, callbackUrl, user.UserName, user.Email, password, user.FirstName, user.LastName);
            SendEmailFromTemplate(user, "Confirm your Email", builder.ToMessageBody());
        }

        public void SendEmailFromTemplate(CBAUser user, string subject, MimeEntity content)
        {
            EmailMessage message = new EmailMessage
            {
                Sender = new MailboxAddress("CBA Admin", _emailMetadata.Sender),
                Reciever = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email),
                Subject = subject,
                Content = content
            };
            var mimeMessage = EmailMessage.CreateEmailMessage(message);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect(_emailMetadata.SmtpServer, _emailMetadata.Port, true);
            smtpClient.Authenticate(_emailMetadata.UserName, _emailMetadata.Password);
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
        }

        public void SendPasswordResetEmail(string pathToFile, string callbackUrl, CBAUser user)
        {
            var builder = new BodyBuilder();
            using StreamReader SourceReader = File.OpenText(pathToFile);
            builder.HtmlBody = SourceReader.ReadToEnd();
            builder.HtmlBody = string.Format(builder.HtmlBody, callbackUrl, user.FirstName);
            SendEmailFromTemplate(user, "Reset your Password", builder.ToMessageBody());
        }

        public async Task UnAssignTill(int tillId)
        {
            var till = await _context.GLAccounts.FindAsync(tillId);
            till.User = null;
            _context.Update(till);
            await _context.SaveChangesAsync();
        }

        public async Task EditUserAsync(string id, UserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.IsEnabled = userViewModel.IsEnabled;
            await _userManager.UpdateAsync(user);
            if (userViewModel.TillId > 0)
            {
                var till = await _context.GLAccounts.FindAsync(userViewModel.TillId);
                till.CBAUserId = user.Id;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserRolesAsync(CBAUser user, List<string> userRoles)
        {
            await _userManager.AddToRolesAsync(user, userRoles);
        }

        public Task<UserViewModel> GetCreateUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserViewModel> GetEditUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userViewModel = new UserViewModel ()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                IsEnabled = user.IsEnabled,
                TillId = -1
            };
            if (user.Till != null)
            {
                userViewModel.TillId = user.Till.GLAccountId;
            }
            var tills = await FetchTills();
            userViewModel.Tills.Add(new SelectListItem()
            {
                Text = "-----",
                Value = "-1"
            });
            foreach (var till in tills)
            {
                userViewModel.Tills.Add(new SelectListItem()
                {
                    Text = "(" + till.AccountNumber.ToString() + ") " + till.AccountName,
                    Value = till.GLAccountId.ToString()
                });
            }
            return userViewModel;
        }
    }
}
