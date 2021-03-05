using CBAData.Models;
using CBAData.Interfaces;  
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAWeb.Controllers
{
    [Authorize(Roles = "Superuser")]
    public class RolesController : Controller
    {
        private readonly RoleManager<CBARole> _roleManager;
        private readonly UserManager<CBAUser> _userManager;
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService, RoleManager<CBARole> roleManager,UserManager<CBAUser> userManager)
        {
            _roleService = roleService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: RolesController
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: RolesController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = await _roleService.RetrieveRoleDetailAsync(id);
            return View(model);
        }

        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                bool isRoleExist = await _roleService.CheckRoleExistsAsync(name);
                if (!isRoleExist)
                {
                    string roleId = await _roleService.CreateRoleAsync(name);
                    return RedirectToAction(nameof(PermissionController.Index), "Permission", new { roleId = roleId });
                }
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = "A role with the same name already exists."
                };
                return View();
            }
            ViewBag.Message = new StatusMessage
            {
                Type = StatusType.Error,
                Message = "The role name cannot be empty"
            };
            return View();
        }

        // GET: RolesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _roleService.RetrieveRoleDetailAsync(id);
            return View(model);
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, RoleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roleService.EditRoleAsync(model.Id, model.IsEnabled, model.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _roleService.CheckRoleExistsAsync(model.Name)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: RolesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: RolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = collection["Id"];
            }
            var role = await _roleManager.FindByIdAsync(id);
            try
            {
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(role);
            }
        }
    }
}
