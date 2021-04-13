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
    public class GLPostingsController : Controller
    {
        private readonly IPostingService _postingService;

        public GLPostingsController(IPostingService postingService)
        {
            _postingService = postingService;
        }

        [Route("GLPostings")]
        [Authorize(Policy = "CBA025")]
        // GET: GLPostingsController
        public async Task<ActionResult> Index()
        {
            var postings = await _postingService.ListGLPostings();
            ViewData["Title"] = "GL Postings";
            return View(postings);
        }

        [Route("GLPostings/{accountCode:long}")]
        [Authorize(Policy = "CBA025")]
        // GET: GLPostingsController/1234567890
        public async Task<ActionResult> Index(long accountCode)
        {
            var postings = await _postingService.GLAccountListPostings(accountCode.ToString());
            ViewData["Title"] = "Transaction History for " + accountCode;
            return View(postings);
        }

        // GET: GLPostingsController/Create
        [Authorize(Policy = "CBA024")]
        public async Task<ActionResult> Create()
        {
            var viewModel = await _postingService.GetCreateGLPosting();
            return View(viewModel);
        }

        // POST: GLPostingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CBA024")]
        public async Task<ActionResult> Create([Bind("Amount,CreditAccountCode,DebitAccountCode,Notes")] GLPostingViewModel viewModel)
        {
            try
            {
                await _postingService.CreateGLPosting(User.FindFirstValue(ClaimTypes.NameIdentifier), viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                ViewBag.Message = new StatusMessage
                {
                    Type = StatusType.Error,
                    Message = "Internal GL Account not found."
                };
                return View(viewModel);
            }
            catch (Exception e)
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
