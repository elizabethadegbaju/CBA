using CBAData;
using CBAData.Interfaces;
using CBAData.Models;
using CBAData.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class PostingService : IPostingService
    {
        private readonly ApplicationDbContext _context;
        public PostingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateGLPosting(string userId, GLPostingViewModel viewModel)
        {
            float creditBalance = 0;
            float debitBalance = 0;
            
            var creditAccount = await _context.InternalAccounts.Include(i=>i.GLCategory)
                .Where(i => i.AccountCode == viewModel.CreditAccountCode)
                .FirstOrDefaultAsync();
            var debitAccount = await _context.InternalAccounts.Include(i=>i.GLCategory)
                .Where(i => i.AccountCode == viewModel.DebitAccountCode)
                .FirstOrDefaultAsync();
            
            switch (creditAccount.GLCategory.Type)
            {
                case AccountType.Assets:
                    creditBalance = creditAccount.AccountBalance - viewModel.Amount;
                    break;
                case AccountType.Liability:
                    creditBalance = creditAccount.AccountBalance + viewModel.Amount;
                    break;
                case AccountType.Capital:
                    creditBalance = creditAccount.AccountBalance + viewModel.Amount;
                    break;
                case AccountType.Income:
                    creditBalance = creditAccount.AccountBalance + viewModel.Amount;
                    break;
                case AccountType.Expense:
                    creditBalance = creditAccount.AccountBalance - viewModel.Amount;
                    break;
                default:
                    break;
            }
            creditAccount.AccountBalance = creditBalance;
            _context.Update(creditAccount);

            switch (debitAccount.GLCategory.Type)
            {
                case AccountType.Assets:
                    debitBalance = debitAccount.AccountBalance + viewModel.Amount;
                    break;
                case AccountType.Liability:
                    debitBalance = debitAccount.AccountBalance - viewModel.Amount;
                    break;
                case AccountType.Capital:
                    debitBalance = debitAccount.AccountBalance - viewModel.Amount;
                    break;
                case AccountType.Income:
                    debitBalance = debitAccount.AccountBalance - viewModel.Amount;
                    break;
                case AccountType.Expense:
                    debitBalance = debitAccount.AccountBalance + viewModel.Amount;
                    break;
                default:
                    break;
            }
            debitAccount.AccountBalance = debitBalance;
            _context.Update(debitAccount);
            await _context.SaveChangesAsync();

            var creditPosting = new Posting
            {
                TransactionDate = viewModel.TransactionDate,
                TransactionId = viewModel.TransactionId,
                Credit = viewModel.Amount,
                Notes = viewModel.Notes,
                AccountCode = viewModel.CreditAccountCode,
                Balance = creditBalance,
                PostingDate = DateTime.Now,
                CBAUserId = userId,
                GLAccount = creditAccount
            };
            _context.Add(creditPosting);

            var debitPosting = new Posting
            {
                TransactionDate = viewModel.TransactionDate,
                TransactionId = viewModel.TransactionId,
                Debit = viewModel.Amount,
                Notes = viewModel.Notes,
                AccountCode = viewModel.DebitAccountCode,
                Balance = debitBalance,
                PostingDate = DateTime.Now,
                CBAUserId = userId,
                GLAccount = debitAccount
            };
            _context.Add(debitPosting);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Posting>> GLAccountListPostings(string AccountCode)
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountCode == AccountCode).ToListAsync();
            return postings;
        }

        public async Task<List<Posting>> ListGLPostings()
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountCode != null).ToListAsync();
            return postings;
        }
    }
}
