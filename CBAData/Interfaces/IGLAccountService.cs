using CBAData.Models;
using CBAData.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IGLAccountService
    {
        public Task AddGLAccountAsync(InternalAccountViewModel accountViewModel);
        public Task EditGLAccountAsync(InternalAccountViewModel accountViewModel);
        public Task<InternalAccount> RetrieveGLAccountAsync(int id);
        public Task DeleteGLAccountAsync(int id);
        public Task ActivateGLAccountAsync(int id);
        public Task DeactivateGLAccountAsync(int id);
        public Task<List<InternalAccount>> ListGLAccountsAsync();
        public Task<bool> GLAccountExists(int id);
        public InternalAccountViewModel GetAddGLAccount();
        public Task<InternalAccountViewModel> GetEditGLAccount(int id);
    }
}
