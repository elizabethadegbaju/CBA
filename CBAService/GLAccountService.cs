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

        public async Task AddGLAccountAsync(InternalAccountViewModel accountViewModel)
        {
            var categoryId = int.Parse(accountViewModel.CategoryId);
            var category = await _context.GLCategories.FindAsync(categoryId);
            var account = new InternalAccount
            {
                AccountCode = GenerateAccountNumber(category.Type),
                AccountName = accountViewModel.AccountName,
                GLCategoryId = categoryId,
                IsActivated = accountViewModel.IsActivated
            };
            _context.Add(account);
            await _context.SaveChangesAsync();
        }

        private string GenerateAccountNumber(AccountType type)
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var start = "";
            switch (type)
            {
                case AccountType.Assets:
                    start = "1";
                    break;
                case AccountType.Liability:
                    start = "2";
                    break;
                case AccountType.Capital:
                    start = "3";
                    break;
                case AccountType.Income:
                    start = "4";
                    break;
                case AccountType.Expense:
                    start = "5";
                    break;
                default:
                    break;
            }
            return start + milliseconds.ToString();
        }

        public async Task EditGLAccountAsync(InternalAccountViewModel accountViewModel)
        {
            var account = await _context.GLAccounts.FindAsync(accountViewModel.Id);
            account.AccountName = accountViewModel.AccountName;
            account.IsActivated = accountViewModel.IsActivated;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<InternalAccount> RetrieveGLAccountAsync(int id)
        {
            var account = await _context.InternalAccounts.Include(account => account.GLCategory).Include(account => account.User).FirstOrDefaultAsync(m => m.GLAccountId == id);
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

        public async Task<List<InternalAccount>> ListGLAccountsAsync()
        {
            var accounts = await _context.InternalAccounts.Include(account => account.GLCategory).Include(account=>account.User).ToListAsync();
            return accounts;
        }
        public async Task<bool> GLAccountExists(int id)
        {
            return await _context.GLAccounts.AnyAsync(e => e.GLAccountId == id);
        }

        public InternalAccountViewModel GetAddGLAccount()
        {
            var accountViewModel = new InternalAccountViewModel();
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
        public async Task<InternalAccountViewModel> GetEditGLAccount(int id)
        {
            var account = await RetrieveGLAccountAsync(id);
            var accountViewModel = new InternalAccountViewModel()
            {
                AccountName = account.AccountName,
                AccountCode = account.AccountCode,
                IsActivated = account.IsActivated,
                CategoryId = account.GLCategoryId.ToString(),
                Id = account.GLAccountId
            };
            accountViewModel.GLCategories.Add(new SelectListItem()
            {
                Text = account.GLCategory.Name,
                Value = account.GLCategory.GLCategoryId.ToString()
            });
            return accountViewModel;
        }
    }
}
