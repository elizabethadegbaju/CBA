using CBAData.Models;
using CBAData.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IPostingService
    {
        public Task<List<Posting>> ListGLPostings();
        public Task CreateGLPosting(string userId, GLPostingViewModel gLPostingViewModel);
    }
}
