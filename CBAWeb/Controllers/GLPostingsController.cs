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

        // GET: PostingsController
        public async Task<ActionResult> Index()
        {
            var postings = await _postingService.ListGLPostings();
            return View(postings);
        }

        // GET: PostingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("")] GLPostingViewModel viewModel)
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
