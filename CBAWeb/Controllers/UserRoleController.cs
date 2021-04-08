using CBAData.Models;
using CBAData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBAData.ViewModels;

namespace CBAWeb.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly SignInManager<CBAUser> _signInManager;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly UserManager<CBAUser> _userManager;
        private readonly IUserService _userService;

        public UserRoleController(SignInManager<CBAUser> signInManager, RoleManager<CBARole> roleManager, UserManager<CBAUser> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
        }

        // GET: UserRolesController
        [Authorize(Policy = "CBA002")]
        public async Task<IActionResult> Index(string userId)
        {
            var model = await _userService.ViewUserRoleAsync(userId);
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
        public async Task<IActionResult> Edit([Bind("User,RoleId,Roles")] UserRoleViewModel model, string id)
        {
            await _userService.EditUserRoleAsync(model);
            return RedirectToAction(nameof(UsersController.Index), "Users");
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
