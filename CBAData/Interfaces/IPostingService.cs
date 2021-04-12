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

        public Task<GLPostingViewModel> GetCreateGLPosting();

        public Task<List<Posting>> GLAccountListPostings(string accountCode);

        public Task<List<Posting>> ListTellerPostings();

        public Task CreateTellerPosting(string userId, TellerPostingViewModel tellerPostingViewModel);

        public Task<TellerPostingViewModel> GetCreateTellerPosting();

        public Task<List<Posting>> CustomerAccountListPostings(string accountNumber);
    }
}
