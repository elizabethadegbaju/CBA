using CBAData.Interfaces;
using CBAData.Models;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CBAWeb.Controllers
{
    public class TellerPostingsController : Controller
    {
        private readonly IPostingService _postingService;

        public TellerPostingsController(IPostingService postingService)
        {
            _postingService = postingService;
        }

        [Route("TellerPostings")]
        [Authorize(Policy = "CBA027")]
        // GET: TellerPostingsController
        public async Task<ActionResult> Index()
        {
            var postings = await _postingService.ListTellerPostings();
            ViewData["Title"] = "Postings to Customer Accounts";
            return View(postings);
        }

        [Route("TellerPostings/{accountNumber:long}")]
        [Authorize(Policy = "CBA027")]
        // GET: TellerPostingsController/1234567890
        public async Task<ActionResult> Index(long accountNumber)
        {
            var postings = await _postingService.CustomerAccountListPostings(accountNumber.ToString());
            ViewData["Title"] = "Transaction History for " + accountNumber;
            return View(postings);
        }

        // GET: TellerPostingsController/Create
        [Authorize(Policy = "CBA026")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TellerPostingsController/Create
        [Authorize(Policy = "CBA026")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("TransactionId,TransactionDate,TransactionType,Amount,AccountNumber,Notes")] TellerPostingViewModel viewModel)
        {
            try
            {
                await _postingService.CreateTellerPosting(User.FindFirstValue(ClaimTypes.NameIdentifier), viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = e.Message
                };
                return View(viewModel);
            }
        }

        // GET: TellerPostingsController/VaultIn
        public ActionResult VaultIn()
        {
            return View();
        }

        // POST: TellerPostingsController/VaultIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VaultIn([Bind("TransactionType,Amount")] TellerPostingViewModel viewModel)
        {
            try
            {
                await _postingService.CreateTellerPosting(User.FindFirstValue(ClaimTypes.NameIdentifier), viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = e.Message
                };
                return View(viewModel);
            }
        }

        // GET: TellerPostingsController/VaultIn
        public ActionResult VaultOut()
        {
            return View();
        }

        // POST: TellerPostingsController/VaultOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VaultOut([Bind("TransactionType,Amount")] TellerPostingViewModel viewModel)
        {
            try
            {
                await _postingService.CreateTellerPosting(User.FindFirstValue(ClaimTypes.NameIdentifier), viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = e.Message
                };
                return View(viewModel);
            }
        }
    }
}

