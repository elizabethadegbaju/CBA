﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CBAData;
using CBAData.Models;
using CBAData.Interfaces;

namespace CBAWeb.Areas.GL.Controllers
{
    [Area("GL")]
    public class AccountConfigurationsController : Controller
    {
        private readonly IAccountConfigurationService _accountConfigurationService;

        public AccountConfigurationsController(IAccountConfigurationService accountConfigurationService)
        {
            _accountConfigurationService = accountConfigurationService;
        }

        // GET: GL/AccountConfigurations
        public async Task<IActionResult> Index()
        {
            var accountConfiguration = await _accountConfigurationService.RetrieveAccountConfiguration();
            return View(accountConfiguration);
        }

        // GET: GL/AccountConfigurations/Edit/5
        public async Task<IActionResult> Edit()
        {
            var accountConfiguration = await _accountConfigurationService.RetrieveAccountConfiguration();
            return View(accountConfiguration);
        }

        // POST: GL/AccountConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,SavingsInterestRate,LoanInterestRate,SavingsMinBalance,CurrentMinBalance,SavingsMaxDailyWithdrawal,CurrentMaxDailyWithdrawal")] AccountConfiguration accountConfiguration)
        {if (ModelState.IsValid)
            {
                try
                {
                    await _accountConfigurationService.UpdateAccountConfiguration(accountConfiguration);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _accountConfigurationService.RetrieveAccountConfiguration() != null)
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
            return View(accountConfiguration);
        }

        // GET: GL/AccountConfigurations/Clear
        public async Task<IActionResult> Clear() 
        {
            await _accountConfigurationService.ClearAccountConfiguration();
            return RedirectToAction(nameof(Index));
        }
    }
}
