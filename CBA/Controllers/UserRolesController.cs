using CBAData.Models;
using CBAService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAWeb.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly SignInManager<CBAUser> _signInManager;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly UserManager<CBAUser> _userManager;
        private readonly IUserService _userService;

        public UserRolesController(SignInManager<CBAUser> signInManager, RoleManager<CBARole> roleManager, UserManager<CBAUser> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
        }

        // GET: UserRolesController
        public async Task<IActionResult> Index(string userId)
        {
            var model = await _userService.ListUserRolesAsync(userId);
            return View(model);
        }

        // GET: UserRolesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: UserRolesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRolesController/Edit/5
        [Authorize(Policy = "CBA002")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManageUserRolesViewModel model, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            _userService.EditUserRolesAsync(id, model.UserRoles, currentUser);
            return RedirectToAction("Index", new { userId = id });
        }

        // GET: UserRolesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
