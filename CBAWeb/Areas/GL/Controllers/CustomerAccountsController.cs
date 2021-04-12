using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CBAData.Models;
using CBAService;
using CBAData.Interfaces;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CBAWeb.Areas.GL.Controllers
{
    [Area("GL")]
    public class CustomerAccountsController : Controller
    {
        private readonly IGLAccountService _gLAccountService;

        public CustomerAccountsController(IGLAccountService gLAccountService)
        {
            _gLAccountService = gLAccountService;
        }

        // GET: GL/CustomerAccounts
        [Authorize(Policy = "CBA014")]
        public async Task<IActionResult> Index()
        {
            var accounts = await _gLAccountService.ListCustomerAccountsAsync();
            return View(accounts);
        }

        // GET: GL/CustomerAccounts/Details/5
        [Authorize(Policy = "CBA014")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAccount = await _gLAccountService.RetrieveCustomerAccountAsync(id.Value);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        // GET: GL/CustomerAccounts/Create
        [Authorize(Policy = "CBA011")]
        public IActionResult Create(int customerId,AccountClass accountClass)
        {
            var viewModel = _gLAccountService.GetAddCustomerAccount(customerId,accountClass);
            return View(viewModel);
        }

        // POST: GL/CustomerAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CBA011")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,AccountClass,AccountName,IsActivated")] CustomerAccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                var account = await _gLAccountService.AddCustomerAccountAsync(accountViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(accountViewModel);
        }

        // GET: GL/CustomerAccounts/Edit/5
        [Authorize(Policy = "CBA012")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountViewModel = await _gLAccountService.GetEditCustomerAccount(id.Value);
            if (accountViewModel == null)
            {
                return NotFound();
            }
            return View(accountViewModel);
        }

        // POST: GL/CustomerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CBA012")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountName,IsActivated")] CustomerAccountViewModel accountViewModel)
        {
            if (id != accountViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gLAccountService.EditCustomerAccountAsync(accountViewModel);
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

        // GET: GL/CustomerAccounts/Delete/5
        [Authorize(Policy = "CBA013")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAccount = await _gLAccountService.RetrieveCustomerAccountAsync(id.Value);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        // POST: GL/CustomerAccounts/Delete/5
        [Authorize(Policy = "CBA013")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gLAccountService.DeleteGLAccountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
