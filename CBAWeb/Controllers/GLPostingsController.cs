using CBAData.Interfaces;
using CBAData.ViewModels;
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
        // GET: GLPostingsController
        public async Task<ActionResult> Index()
        {
            var postings = await _postingService.ListGLPostings();
            ViewData["Title"] = "GL Postings";
            return View(postings);
        }

        [Route("GLPostings/{accountCode:long}")]
        // GET: GLPostingsController/1234567890
        public async Task<ActionResult> Index(long accountCode)
        {
            var postings = await _postingService.GLAccountListPostings(accountCode.ToString());
            ViewData["Title"] = "Transaction History for " + accountCode;
            return View(postings);
        }

        // GET: GLPostingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GLPostingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("TransactionId,TransactionDate,Amount,DebitAccountCode,DebitAccountCode,Notes")] GLPostingViewModel viewModel)
        {
            try
            {
                await _postingService.CreateGLPosting(User.FindFirstValue(ClaimTypes.NameIdentifier), viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
