using CBAData;
using CBAData.Interfaces;
using CBAData.Models;
using CBAData.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class GLAccountService : IGLAccountService
    {
        private readonly ApplicationDbContext _context;

        public GLAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddGLAccountAsync(AccountViewModel accountViewModel)
        {
            var account = new GLAccount
            {
                AccountName = accountViewModel.AccountName,
                GLCategoryId = int.Parse(accountViewModel.CategoryId),
                IsActivated = accountViewModel.IsActivated
            };
            _context.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task EditGLAccountAsync(GLAccount account)
        {
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<GLAccount> RetrieveGLAccountAsync(int id)
        {
            var account = await _context.GLAccounts.FirstOrDefaultAsync(m => m.GLAccountId == id);
            return account;
        }

        public async Task DeleteGLAccountAsync(int id)
        {
            var account = RetrieveGLAccountAsync(id).Result;
            _context.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task ActivateGLAccountAsync(int id)
        {
            var account = RetrieveGLAccountAsync(id).Result;
            account.IsActivated = true;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateGLAccountAsync(int id)
        {
            var account = RetrieveGLAccountAsync(id).Result;
            account.IsActivated = false;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GLAccount>> ListGLAccountsAsync()
        {
            var accounts = await _context.GLAccounts.Include(account => account.GLCategory).ToListAsync();
            return accounts;
        }

        public async Task<bool> GLAccountExists(int id)
        {
            return await _context.GLAccounts.AnyAsync(e => e.GLAccountId == id);
        }

        public AccountViewModel GetAddGLAccount()
        {
            var accountViewModel = new AccountViewModel();
            var categories = _context.GLCategories.ToList();
            foreach (var category in categories)
            {
                accountViewModel.GLCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.GLCategoryId.ToString()
                });
            }
            return accountViewModel;
        }
    }
}
