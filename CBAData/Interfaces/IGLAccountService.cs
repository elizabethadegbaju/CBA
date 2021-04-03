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
        public Task AddGLAccountAsync(AccountViewModel accountViewModel);
        public Task EditGLAccountAsync(GLAccount account);
        public Task<GLAccount> RetrieveGLAccountAsync(int id);
        public Task DeleteGLAccountAsync(int id);
        public Task ActivateGLAccountAsync(int id);
        public Task DeactivateGLAccountAsync(int id);
        public Task<List<GLAccount>> ListGLAccountsAsync();
        public Task<bool> GLAccountExists(int id);
        public AccountViewModel GetAddGLAccount();
    }
}
