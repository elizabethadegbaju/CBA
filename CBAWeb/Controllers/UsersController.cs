using CBAData.Models;
using CBAData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CBAWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<CBAUser> _userManager;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, UserManager<CBAUser> userManager, RoleManager<CBARole> roleManager, IWebHostEnvironment env)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        // GET: UsersController
        [Authorize(Policy = "CBA005")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allOtherUsers = await _userService.ListUsersExceptSpecifiedUserAsync(currentUser);
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<StatusMessage>((string)TempData["Message"]);
            }
            return View(allOtherUsers);
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
            var model = _userService.LoadEmptyUser();
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
                    var password = Password.Generate(10, 4);
                    var user = await _userService.CreateUserAsync(model.User, password);
                    var userRoles = model.UserRoles.Where(a => a.IsSelected).Select(b => b.Name);
                    await _userService.UpdateUserRolesAsync(user, userRoles.ToList());

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(ConfirmEmail), "Users", new { userId = user.Id, code = code }, HttpContext.Request.Scheme);
                    var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplate"
                        + Path.DirectorySeparatorChar.ToString()
                        + "ConfirmAccountRegistration.html";
                    _userService.SendAccountConfirmationEmail(pathToFile, callbackUrl, user, password);

                    ViewBag.Message = new StatusMessage
                    {
                        Type = StatusType.Success,
                        Message = "User created successfully."
                    };
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception error)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = error.Message
                };
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
            try
            {
                await _userService.DeleteUserAsync(id);
                var message = new StatusMessage
                {
                    Type = StatusType.Success,
                    Message = "User deleted successfully"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception error)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = error.Message
                };
                return View(await _userManager.FindByIdAsync(id));
            }
        }

        // GET: UsersController/ConfirmEmail
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error", new ErrorViewModel());
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var message = new StatusMessage();
            if (result.Errors.Any())
            {
                message.Type = StatusType.Error;
                message.Message = "Unable to confirm your email.";
            }
            else
            {
                message.Type = StatusType.Success;
                message.Message = "Your email has been confirmed. You can log in now.";
            }
            TempData["Message"] = JsonConvert.SerializeObject(message);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
