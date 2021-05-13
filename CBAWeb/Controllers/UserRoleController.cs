using CBAData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CBAData.ViewModels;

namespace CBAWeb.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserService _userService;

        public UserRoleController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserRolesController
        [Authorize(Policy = "CBA002")]
        public async Task<IActionResult> Index(string userId)
        {
            var model = await _userService.ViewUserRoleAsync(userId);
            return View(model);
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
    }
}
