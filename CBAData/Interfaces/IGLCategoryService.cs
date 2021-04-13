using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IGLCategoryService
    {
        public Task AddGLCategoryAsync(GLCategory category);
        public Task EditGLCategoryAsync(GLCategory category);
        public Task<GLCategory> RetrieveGLCategoryAsync(int id);
        public Task DeleteGLCategoryAsync(int id);
        public Task EnableGLCategoryAsync(int id);
        public Task DisableGLCategoryAsync(int id);
        public Task<List<GLCategory>> ListGLCategoriesAsync();
        public Task<bool> GLCategoryExists(int id);
    }
}
