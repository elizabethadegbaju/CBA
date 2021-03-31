using CBAData;
using CBAData.Interfaces;
using CBAData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class GLCategoryService : IGLCategoryService
    {
        private readonly ApplicationDbContext _context;

        public GLCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddGLCategoryAsync(string name, AccountType accountType)
        {
            var category = new GLCategory
            {
                Name = name,
                AccountType = accountType
            };
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGLCategoryAsync(int id)
        {
            var category = RetrieveGLCategoryAsync(id).Result;
            _context.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task DisableGLCategoryAsync(int id)
        {
            var category = RetrieveGLCategoryAsync(id).Result;
            category.IsEnabled = false;
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task EditGLCategoryAsync(GLCategory category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task EnableGLCategoryAsync(int id)
        {
            var category = RetrieveGLCategoryAsync(id).Result;
            category.IsEnabled = true;
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GLCategory>> ListGLCategoriesAsync()
        {
            return await _context.GLCategories.ToListAsync();
        }

        public async Task<GLCategory> RetrieveGLCategoryAsync(int id)
        {
            var category = await _context.GLCategories.FirstOrDefaultAsync(m => m.Id == id);
            return category;
        }

        public async Task<bool> GLCategoryExists(int id)
        {
            return await _context.GLCategories.AnyAsync(e => e.Id == id);
        }
    }
}
