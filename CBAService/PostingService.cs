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

            var now = DateTime.Now;
            var creditPosting = new Posting
            {
                TransactionDate = now,
                Credit = viewModel.Amount,
                Notes = viewModel.Notes,
                AccountCode = viewModel.CreditAccountCode,
                Balance = creditBalance,
                PostingDate = now,
                CBAUserId = userId,
                GLAccount = creditAccount
            };
            _context.Add(creditPosting);

            var debitPosting = new Posting
            {
                TransactionDate = now,
                Debit = viewModel.Amount,
                Notes = viewModel.Notes,
                AccountCode = viewModel.DebitAccountCode,
                Balance = debitBalance,
                PostingDate = now,
                CBAUserId = userId,
                GLAccount = debitAccount
            };
            _context.Add(debitPosting);
            await _context.SaveChangesAsync();
        }

        public async Task CreateTellerPosting(string userId, TellerPostingViewModel viewModel)
        {
            float creditBalance = 0;
            float debitBalance = 0;

            CBAUser user = await _context.Users.Include(u => u.Till).FirstOrDefaultAsync(u => u.Id == userId);
            InternalAccount tellerAccount = user.Till;
            if (tellerAccount==null)
            {
                throw new Exception("You cannot post  a customer transaction. There is no Till associated with this account.");
            }
            if (await _context.Postings.AnyAsync(p => p.TransactionSlipNo == viewModel.TransactionSlipNo))
            {
                throw new Exception("Duplicate Transaction. A transaction with this Slip Number has already been posted.");
            }

            InternalAccount vault = await _context.InternalAccounts.SingleOrDefaultAsync(i => i.AccountCode == "10000000000000");
            AccountConfiguration settings = _context.AccountConfigurations.First();
            CustomerAccount customerAccount;

            var creditPosting = new Posting
            {
                TransactionDate = viewModel.TransactionDate,
                TransactionSlipNo = viewModel.TransactionSlipNo,
                Credit = viewModel.Amount,
                Notes = viewModel.Notes,
                Balance = creditBalance,
                PostingDate = DateTime.Now,
                CBAUserId = userId,
            };

            var debitPosting = new Posting
            {
                TransactionDate = viewModel.TransactionDate,
                TransactionSlipNo = viewModel.TransactionSlipNo,
                Debit = viewModel.Amount,
                Notes = viewModel.Notes,
                Balance = debitBalance,
                PostingDate = DateTime.Now,
                CBAUserId = userId,
            };

            switch (viewModel.TransactionType)
            {
                case TransactionType.Withdrawal:
                    customerAccount = await _context.CustomerAccounts
                        .Where(i => i.AccountNumber == viewModel.AccountNumber)
                        .FirstOrDefaultAsync();
                    if (!customerAccount.IsActivated)
                    {
                        throw new Exception("Unable to post transaction. Customer Account is deactivated.");
                    }
                    if (tellerAccount.AccountBalance < viewModel.Amount)
                    {
                        throw new Exception("Unable to post transaction. Insufficient funds in Till.");
                    }
                    switch (customerAccount.AccountClass)
                    {
                        case AccountClass.Savings:
                            if (customerAccount.AccountBalance <= settings.SavingsMinBalance)
                            {
                                throw new Exception("Account has reached Minimum Balance limit. Cannot post withdrawal.");
                            }
                            break;
                        case AccountClass.Current:
                            if (customerAccount.AccountBalance <= settings.CurrentMinBalance)
                            {
                                throw new Exception("Account has reached Minimum Balance limit. Cannot post withdrawal.");
                            }
                            break;
                        case AccountClass.Loan:
                            break;
                        default:
                            break;
                    }
                    creditBalance = tellerAccount.AccountBalance - viewModel.Amount;
                    debitBalance = customerAccount.AccountBalance - viewModel.Amount;

                    creditPosting.AccountCode = tellerAccount.AccountCode;
                    debitPosting.AccountNumber = customerAccount.AccountNumber;

                    creditPosting.GLAccount = tellerAccount;
                    debitPosting.GLAccount = customerAccount;

                    tellerAccount.AccountBalance = creditBalance;
                    customerAccount.AccountBalance = debitBalance;
                    _context.Update(customerAccount);
                    break;
                case TransactionType.Deposit:
                    customerAccount = await _context.CustomerAccounts
                        .Where(i => i.AccountNumber == viewModel.AccountNumber)
                        .FirstOrDefaultAsync();
                    if (!customerAccount.IsActivated)
                    {
                        throw new Exception("Unable to post transaction. Customer Account is deactivated.");
                    }
                    creditBalance = customerAccount.AccountBalance + viewModel.Amount;
                    debitBalance = tellerAccount.AccountBalance + viewModel.Amount;

                    creditPosting.AccountNumber = customerAccount.AccountNumber;
                    debitPosting.AccountCode = tellerAccount.AccountCode;

                    creditPosting.GLAccount = customerAccount;
                    debitPosting.GLAccount = tellerAccount;

                    tellerAccount.AccountBalance = debitBalance;
                    customerAccount.AccountBalance = creditBalance;
                    _context.Update(customerAccount);
                    break;
                case TransactionType.VaultIn:
                    creditBalance = tellerAccount.AccountBalance - viewModel.Amount;
                    debitBalance = vault.AccountBalance + viewModel.Amount;

                    creditPosting.AccountCode = tellerAccount.AccountCode;
                    debitPosting.AccountCode = vault.AccountCode;

                    creditPosting.GLAccount = tellerAccount;
                    debitPosting.GLAccount = vault;

                    tellerAccount.AccountBalance = creditBalance;
                    vault.AccountBalance = debitBalance;
                    _context.Update(vault);
                    break;
                case TransactionType.VaultOut:
                    creditBalance = vault.AccountBalance + viewModel.Amount;
                    debitBalance = tellerAccount.AccountBalance - viewModel.Amount;

                    creditPosting.AccountCode = vault.AccountCode;
                    debitPosting.AccountCode = tellerAccount.AccountCode;

                    creditPosting.GLAccount = vault;
                    debitPosting.GLAccount = tellerAccount;

                    tellerAccount.AccountBalance = debitBalance;
                    vault.AccountBalance = creditBalance;
                    _context.Update(vault);
                    break;
                default:
                    break;
            }

            _context.Update(tellerAccount);
            await _context.SaveChangesAsync();

            creditPosting.TransactionDate = DateTime.Now;
            debitPosting.TransactionDate = DateTime.Now;
            _context.Add(creditPosting);
            _context.Add(debitPosting);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Posting>> CustomerAccountListPostings(string accountNumber)
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountNumber == accountNumber).ToListAsync();
            return postings;
        }

        public async Task<List<Posting>> GLAccountListPostings(string accountCode)
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountCode == accountCode).ToListAsync();
            return postings;
        }

        public async Task<List<Posting>> ListGLPostings()
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountCode != null).ToListAsync();
            return postings;
        }

        public async Task<List<Posting>> ListTellerPostings()
        {
            var postings = await _context.Postings.Include(p=>p.PostedBy).Where(p => p.AccountNumber != null).ToListAsync();
            return postings;
        }
    }
}
