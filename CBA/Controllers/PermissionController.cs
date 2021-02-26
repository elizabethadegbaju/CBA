using CBA.Models;
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

namespace CBA.Controllers
{
    [Authorize(Roles = "Superuser")]
    public class PermissionController : Controller
    {
        private readonly RoleManager<CBARole> _roleManager;

        public PermissionController(RoleManager<CBARole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: PermissionController
        public async Task<IActionResult> Index(string roleId)
        {
            var permissions = new List<RoleClaimsViewModel>();
            MemberInfo[] permissionEnum = typeof(Permissions).GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var permission in permissionEnum)
            {
                var name = ((DisplayAttribute)permission.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                permissions.Add(new RoleClaimsViewModel { Value = name, Type = permission.Name });
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            var model = new PermissionViewModel()
            {
                Role = role
            };
            var claims = await _roleManager.GetClaimsAsync(role);
            var claimValues = permissions.Select(permission => permission.Type).ToList();
            var roleClaimValues = claims.Select(claim => claim.Type).ToList();
            var authorizedClaims = claimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in permissions)
            {
                if (authorizedClaims.Any(a => a == permission.Type))
                {
                    permission.IsSelected = true;
                }
            }
            model.RoleClaims = permissions;
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
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.IsSelected).ToList();
            foreach (var claim in selectedClaims)
            {
                MemberInfo member = typeof(Permissions).GetMember(claim.Type)[0];
                var name = ((DisplayAttribute)member.GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
                await _roleManager.AddClaimAsync(role, new Claim(claim.Type, name));
            }
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
