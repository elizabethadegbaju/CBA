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

        public async Task<CBAUser> CreateUserAsync(CBAUser modelUser, string password)
        {
            MailAddress address = new MailAddress(modelUser.Email);
            string userName = address.User;
            var user = new CBAUser
            {
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

        public async Task EditUserRoleAsync(UserRoleViewModel userRoleViewModel)
        {
            CBAUser user = await _userManager.FindByIdAsync(userRoleViewModel.User.Id);
            CBARole role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRoleAsync(user, role.Name);
            user.CBARole = role;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRoleViewModel> ViewUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoleViewModel = new UserRoleViewModel()
            {
                RoleId = user.CBARoleId,
                User = user
            };
            foreach (var role in _roleManager.Roles)
            {
                userRoleViewModel.Roles.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id
                });
            }
            return userRoleViewModel;
        }

        public async Task<List<CBAUser>> ListUsersAsync()
        {
            var users = await _context.Users.Include(u => u.CBARole).ToListAsync();
            return users;
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

        public async Task UpdateUserAsync(string id, CBAUser user)
        {
            var formerUser = await _userManager.FindByIdAsync(id);
            formerUser.FirstName = user.FirstName;
            formerUser.LastName = user.LastName;
            formerUser.IsEnabled = user.IsEnabled;
            await _userManager.UpdateAsync(formerUser);
        }

        public async Task UpdateUserRoleAsync(CBAUser user, string role)
        {
            user.CBARole = await _roleManager.FindByNameAsync(role);
            _context.Update(user);
            await _context.SaveChangesAsync();
            await _userManager.AddToRoleAsync(user, role);
        }

        public UserRoleViewModel GetCreateUserAsync()
        {
            var userRoleViewModel = new UserRoleViewModel();
            foreach (var role in _roleManager.Roles)
            {
                userRoleViewModel.Roles.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id
                });
            }
            return userRoleViewModel;
        }
    }
}
