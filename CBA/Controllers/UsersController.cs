using CBA.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CBA.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<CBAUser> _userManager;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly EmailMetadata _emailMetadata;

        public UsersController(UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager, EmailMetadata emailMetadata)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailMetadata = emailMetadata;
        }

        // GET: UsersController
        [Authorize(Policy = "CBA005")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allOtherUsers = _userManager.Users.Where(user => user.Id != currentUser.Id);
            return View(await allOtherUsers.ToListAsync());
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        [Authorize(Policy = "CBA001")]
        public ActionResult Create()
        {
            var viewModel = new List<UserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    Name = role.Name,
                    IsSelected = false
                };
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesViewModel()
            {
                User = new CBAUser(),
                UserRoles = viewModel
            };
            return View(model);
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CBA001")]
        public async Task<ActionResult> Create(ManageUserRolesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelUser = model.User;
                    MailAddress address = new MailAddress(modelUser.Email);
                    string userName = address.User;
                    var userRoles = model.UserRoles.Where(a => a.IsSelected).Select(b => b.Name);
                    var user = new CBAUser { UserName = userName, Email = modelUser.Email, FirstName = modelUser.FirstName, LastName = modelUser.LastName };
                    var password = Password.Generate(10, 4);
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRolesAsync(user, userRoles);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action(nameof(ConfirmEmail), "Users", new { area = "Identity", userId = user.Id, code = code },                            HttpContext.Request.Scheme);
                        EmailMessage message = new EmailMessage
                        {
                            Sender = new MailboxAddress("CBA Admin", _emailMetadata.Sender),
                            Reciever = new MailboxAddress($"{user.FirstName} {user.LastName}", modelUser.Email),
                            Subject = "Confirm your email",
                            Content = $"<p>Username={userName}</p>" +
                            $"<p>Password={password}</p>" +
                            $"<p>Don't forget to change your password.</p>" +
                            $"<br />" +
                            $"<p>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>"
                        };
                        var mimeMessage = EmailMessage.CreateEmailMessage(message);
                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            smtpClient.Connect(_emailMetadata.SmtpServer,
                            _emailMetadata.Port, true);
                            smtpClient.Authenticate(_emailMetadata.UserName,
                            _emailMetadata.Password);
                            smtpClient.Send(mimeMessage);
                            smtpClient.Disconnect(true);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(model);
            }
            catch (Exception error)
            {
                ModelState.AddModelError(string.Empty, error.Message);
                return View(model);
            }
        }

        // GET: UsersController/Edit/5
        [Authorize(Policy = "CBA002")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [Authorize(Policy = "CBA002")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        [Authorize(Policy = "CBA002")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CBA002")]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = collection["Id"];
            }
            var user = await _userManager.FindByIdAsync(id);
            try
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(user);
            }

        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewData["Message"] = "Your email has been confirmed. Please Login now. ";
            ViewData["MessageValue"] = "1";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
