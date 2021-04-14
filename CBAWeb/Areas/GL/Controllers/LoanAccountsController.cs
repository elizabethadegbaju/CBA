using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBAData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CBAData.ViewModels;
using CBAData.Models;
using Microsoft.EntityFrameworkCore;

namespace CBAWeb.Areas.GL.Controllers
{
    [Area("GL")]
    public class LoanAccountsController : Controller
    {
        private readonly IGLAccountService _gLAccountService;

        public LoanAccountsController(IGLAccountService gLAccountService)
        {
            _gLAccountService = gLAccountService;
        }

        // GET: GL/LoanAccounts
        [Authorize(Policy = "CBA014")]
        public async Task<IActionResult> Index()
        {
            var accounts = await _gLAccountService.ListLoanAccountsAsync();
            return View(accounts);
        }

        // GET: GL/LoanAccounts/Details/5
        [Authorize(Policy = "CBA014")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanAccount = await _gLAccountService.RetrieveLoanAccountAsync(id.Value);
            if (loanAccount == null)
            {
                return NotFound();
            }

            return View(loanAccount);
        }

        // GET: GL/LoanAccounts/Create
        [Authorize(Policy = "CBA011")]
        public async Task<IActionResult> Create(string linkedAccountNumber)
        {
            var viewModel = await _gLAccountService.GetAddLoanAccount(linkedAccountNumber);
            return View(viewModel);
        }

        // POST: GL/LoanAccounts/Create
        [Authorize(Policy = "CBA011")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GLAccountId,CustomerAccounts,InterestRate,AccountName,LinkedAccount,Principal,DurationYears,RepaymentFrequencyMonths,StartDate")] LoanAccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _gLAccountService.AddLoanAccountAsync(accountViewModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = new StatusMessage
                    {
                        Message = ex.Message,
                        Type = StatusType.Error
                    };
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(accountViewModel);
        }

        // GET: GL/LoanAccounts/Edit/5
        [Authorize(Policy = "CBA012")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accountViewModel = await _gLAccountService.GetEditLoanAccount(id.Value);
            if (accountViewModel == null)
            {
                return NotFound();
            }
            return View(accountViewModel);
        }

        // POST: GL/LoanAccounts/Edit/5
        [Authorize(Policy = "CBA012")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GLAccountId,CustomerAccounts,InterestRate,AccountName,LinkedAccount,Principal,DurationYears,RepaymentFrequencyMonths,StartDate")] LoanAccountViewModel accountViewModel)
        {
            if (id != accountViewModel.GLAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gLAccountService.EditLoanAccountAsync(accountViewModel);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gLAccountService.GLAccountExists(accountViewModel.GLAccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = new StatusMessage
                    {
                        Message = ex.Message,
                        Type = StatusType.Error
                    };
                    return View(accountViewModel);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(accountViewModel);
        }

        // GET: GL/LoanAccounts/Delete/5
        [Authorize(Policy = "CBA013")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanAccount = await _gLAccountService.RetrieveLoanAccountAsync(id.Value);
            if (loanAccount == null)
            {
                return NotFound();
            }

            return View(loanAccount);
        }

        // POST: GL/LoanAccounts/Delete/5
        [Authorize(Policy = "CBA013")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _gLAccountService.DeleteGLAccountAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = new StatusMessage
                {
                    Message = ex.Message,
                    Type = StatusType.Error
                };
                return View();
            }
        }
    }
}