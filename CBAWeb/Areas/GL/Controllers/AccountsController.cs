using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CBAData;
using CBAData.Models;
using CBAData.Interfaces;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CBAWeb.Areas.GL.Controllers
{
    [Area("GL")]
    [Authorize(Policy = "CBA023")]
    public class AccountsController : Controller
    {
        private readonly IGLAccountService _gLAccountService;

        public AccountsController(IGLAccountService gLAccountService)
        {
            _gLAccountService = gLAccountService;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _gLAccountService.ListGLAccountsAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLAccount = await _gLAccountService.RetrieveGLAccountAsync(id.Value);
            if (gLAccount == null)
            {
                return NotFound();
            }

            return View(gLAccount);
        }

        // GET: Accounts/Create
        [Authorize(Policy = "CBA021")]
        public IActionResult Create()
        {
            var model = _gLAccountService.GetAddGLAccount();
            return View(model);
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CBA021")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,AccountName,IsActivated")] AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                await _gLAccountService.AddGLAccountAsync(accountViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(accountViewModel);
        }

        // GET: Accounts/Edit/5
        [Authorize(Policy = "CBA022")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLAccountViewModel = await _gLAccountService.GetEditGLAccount(id.Value);
            if (gLAccountViewModel == null)
            {
                return NotFound();
            }
            return View(gLAccountViewModel);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CBA022")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountName,IsActivated")] AccountViewModel accountViewModel)
        {
            if (id != accountViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gLAccountService.EditGLAccountAsync(accountViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gLAccountService.GLAccountExists(accountViewModel.Id))
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
            return View(accountViewModel);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLAccount = await _gLAccountService.RetrieveGLAccountAsync(id.Value);
            if (gLAccount == null)
            {
                return NotFound();
            }

            return View(gLAccount);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gLAccountService.DeleteGLAccountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
