using CBAData.Interfaces;
using CBAData.Models;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CBAWeb.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<CBARole> _roleManager;
        private readonly UserManager<CBAUser> _userManager;

        public PermissionController(IPermissionService permissionService, RoleManager<CBARole> roleManager, UserManager<CBAUser> userManager)
        {
            _permissionService = permissionService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: PermissionController
        public async Task<IActionResult> Index(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var model = new PermissionViewModel()
            {
                Role = role,
                RoleClaims = await _permissionService.ListRoleClaimsAsync(role)
            };
            return View(model);
        }

        // POST: PermissionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Role.Id);
            var user = await _userManager.GetUserAsync(User);
            await _permissionService.EditRoleClaimsAsync(role, model.RoleClaims, user);
            return RedirectToAction(nameof(RolesController.Index), "Roles");
        }
    }
}
