using CBA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBA.Controllers
{
    [Authorize(Roles = "Superuser")]
    public class RolesController : Controller
    {
        private readonly RoleManager<CBARole> _roleManager;

        public RolesController(RoleManager<CBARole> roleManager)
        {
            _roleManager = roleManager;
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
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
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
            CBARole role;
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    await _roleManager.CreateAsync(new CBARole(name));
                }
                catch
                {
                    return View();
                }
            }
            role = await _roleManager.FindByNameAsync(name);
            return RedirectToAction(nameof(PermissionController.Index), "Permission", new { roleId = role.Id });
        }

        // GET: RolesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RolesController/Edit/5
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
