using CBAData.Interfaces;
using CBAData.Models;
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
    [Authorize(Roles = "Superuser")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<CBARole> _roleManager;

        public PermissionController(IPermissionService permissionService, RoleManager<CBARole> roleManager)
        {
            _permissionService = permissionService;
            _roleManager = roleManager;
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

        // GET: PermissionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PermissionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PermissionController/Create
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

        // GET: PermissionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PermissionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Role.Id);
            await _permissionService.EditRoleClaimsAsync(role, model.RoleClaims);
            return RedirectToAction("Index", new { roleId = model.Role.Id });
        }

        // GET: PermissionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PermissionController/Delete/5
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
