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

namespace CBAWeb.Areas.GL.Controllers
{
    [Area("GL")]
    public class CategoriesController : Controller
    {
        private readonly IGLCategoryService _gLCategoryService;

        public CategoriesController(IGLCategoryService gLCategoryService)
        {
            _gLCategoryService = gLCategoryService;
        }

        // GET: GLCategories
        public async Task<IActionResult> Index()
        {
            return View(await _gLCategoryService.ListGLCategoriesAsync());
        }

        // GET: GLCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLCategory = await _gLCategoryService.RetrieveGLCategoryAsync(id.Value);
            if (gLCategory == null)
            {
                return NotFound();
            }

            return View(gLCategory);
        }

        // GET: GLCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GLCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountType,Name,IsEnabled")] GLCategory gLCategory)
        {
            if (ModelState.IsValid)
            {
                await _gLCategoryService.AddGLCategoryAsync(gLCategory.Name, gLCategory.AccountType);
                return RedirectToAction(nameof(Index));
            }
            return View(gLCategory);
        }

        // GET: GLCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLCategory = await _gLCategoryService.RetrieveGLCategoryAsync(id.Value);
            if (gLCategory == null)
            {
                return NotFound();
            }
            return View(gLCategory);
        }

        // POST: GLCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountType,Name,IsEnabled")] GLCategory gLCategory)
        {
            if (id != gLCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gLCategoryService.EditGLCategoryAsync(gLCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gLCategoryService.GLCategoryExists(gLCategory.Id))
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
            return View(gLCategory);
        }

        // GET: GLCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gLCategory = await _gLCategoryService.RetrieveGLCategoryAsync(id.Value);
            if (gLCategory == null)
            {
                return NotFound();
            }

            return View(gLCategory);
        }

        // POST: GLCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gLCategoryService.DeleteGLCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
