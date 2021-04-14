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

        public async Task AddInternalAccountAsync(InternalAccountViewModel accountViewModel)
        {
            var categoryId = int.Parse(accountViewModel.CategoryId);
            var category = await _context.GLCategories.FindAsync(categoryId);
            var account = new InternalAccount
            {
                AccountCode = GenerateInternalAccountCode(category.Type),
                AccountName = accountViewModel.AccountName,
                GLCategoryId = categoryId,
                IsActivated = accountViewModel.IsActivated
            };
            _context.Add(account);
            await _context.SaveChangesAsync();
        }

        private string GenerateInternalAccountCode(AccountType type)
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

        private string GenerateCustomerAccountNumber(AccountClass accountClass, int customerId, int accountId)
        {
            int classId = (int)accountClass;
            string accountNumber = classId.ToString() + customerId.ToString("D4") + accountId.ToString("D6");
            return accountNumber;
        }

        public async Task EditInternalAccountAsync(InternalAccountViewModel accountViewModel)
        {
            var account = await _context.GLAccounts.FindAsync(accountViewModel.Id);
            account.AccountName = accountViewModel.AccountName;
            account.IsActivated = accountViewModel.IsActivated;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<InternalAccount> RetrieveInternalAccountAsync(int id)
        {
            var account = await _context.InternalAccounts.Include(account => account.GLCategory).Include(account => account.User).FirstOrDefaultAsync(m => m.GLAccountId == id);
            return account;
        }

        public async Task DeleteGLAccountAsync(int id)
        {
            var account = RetrieveInternalAccountAsync(id).Result;
            _context.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task ActivateGLAccountAsync(int id)
        {
            var account = RetrieveInternalAccountAsync(id).Result;
            account.IsActivated = true;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateGLAccountAsync(int id)
        {
            var account = RetrieveInternalAccountAsync(id).Result;
            account.IsActivated = false;
            _context.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InternalAccount>> ListInternalAccountsAsync()
        {
            var accounts = await _context.InternalAccounts.Include(account => account.GLCategory).Include(account => account.User).ToListAsync();
            return accounts;
        }

        public async Task<bool> GLAccountExists(int id)
        {
            return await _context.GLAccounts.AnyAsync(e => e.GLAccountId == id);
        }

        public InternalAccountViewModel GetAddInternalAccount()
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

        public async Task<InternalAccountViewModel> GetEditInternalAccount(int id)
        {
            var account = await RetrieveInternalAccountAsync(id);
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

        public async Task<CustomerAccount> AddCustomerAccountAsync(CustomerAccountViewModel accountViewModel)
        {
            var customer = await _context.Customers.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.CustomerId == accountViewModel.CustomerId);
            if (customer.Accounts.Any(a => a.AccountClass == accountViewModel.AccountClass))
            {
                throw new Exception($"Unable to create account. Customer already has a {accountViewModel.AccountClass} account in the system.");
            }
            if (accountViewModel.AccountClass==AccountClass.Loan)
            {
                throw new Exception("Wrong account class selected.");
            }
            var customerAccount = new CustomerAccount()
            {
                CustomerId = accountViewModel.CustomerId,
                IsActivated = accountViewModel.IsActivated,
                AccountClass = accountViewModel.AccountClass,
                AccountName = $"{customer.FirstName} {customer.LastName} {accountViewModel.AccountClass}",
                DateOpened = DateTime.Now
            };
            _context.Add(customerAccount);
            await _context.SaveChangesAsync();
            customerAccount.AccountNumber = GenerateCustomerAccountNumber(customerAccount.AccountClass, customerAccount.CustomerId, customerAccount.GLAccountId);
            _context.Update(customerAccount);
            await _context.SaveChangesAsync();
            return customerAccount;
        }

        public async Task EditCustomerAccountAsync(CustomerAccountViewModel accountViewModel)
        {
            var customerAccount = await RetrieveCustomerAccountAsync(accountViewModel.Id);
            customerAccount.AccountName = accountViewModel.AccountName;
            customerAccount.IsActivated = accountViewModel.IsActivated;
            _context.Update(customerAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<CustomerAccount> RetrieveCustomerAccountAsync(int id)
        {
            var account = await _context.CustomerAccounts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.GLAccountId == id);
            return account;
        }

        public async Task<List<CustomerAccount>> ListCustomerAccountsAsync()
        {
            var accounts = _context.CustomerAccounts.Include(c => c.Customer);
            return await accounts.ToListAsync();
        }

        public CustomerAccountViewModel GetAddCustomerAccount(int customerId, AccountClass accountClass)
        {
            var viewModel = new CustomerAccountViewModel
            {
                CustomerId = customerId,
                AccountClass = accountClass
            };
            return viewModel;
        }

        public async Task<CustomerAccountViewModel> GetEditCustomerAccount(int id)
        {
            var account = await RetrieveCustomerAccountAsync(id);
            var viewModel = new CustomerAccountViewModel
            {
                Id = account.GLAccountId,
                CustomerId = account.CustomerId,
                AccountClass = account.AccountClass,
                IsActivated = account.IsActivated,
                AccountName = account.AccountName,
                AccountNumber = account.AccountNumber
            };
            return viewModel;
        }

        public async Task<LoanAccount> AddLoanAccountAsync(LoanAccountViewModel accountViewModel)
        {
            var customer = await _context.Customers.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.CustomerId == accountViewModel.LinkedAccount.CustomerId);
            var settings = _context.AccountConfigurations.First();
            if (customer.Accounts.Any(a => a.AccountClass == AccountClass.Loan))
            {
                throw new Exception($"Unable to create account. Customer already has a Loan account in the system.");
            }
            var loanAccount = new LoanAccount()
            {
                CustomerAccount = accountViewModel.LinkedAccount,
                IsActivated = true,
                AccountClass = AccountClass.Loan,
                AccountName = $"{customer.FirstName} {customer.LastName} Loan",
                DateOpened = DateTime.Now,
                Principal = accountViewModel.Principal,
                InterestRate = (double)settings.LoanInterestRate,
                DurationYears = accountViewModel.DurationYears,
                RepaymentFrequencyMonths = accountViewModel.RepaymentFrequencyMonths,
                StartDate = accountViewModel.StartDate,
                Customer = accountViewModel.LinkedAccount.Customer
            };
            _context.Add(loanAccount);
            var noPaymentsPerYear = 12 / loanAccount.RepaymentFrequencyMonths;
            loanAccount.CompoundInterest = (loanAccount.Principal * Math.Pow((1 + (loanAccount.InterestRate / 100) / (noPaymentsPerYear)), (noPaymentsPerYear * loanAccount.DurationYears))) - loanAccount.Principal;
            loanAccount.AccountNumber = GenerateCustomerAccountNumber(loanAccount.AccountClass, loanAccount.CustomerId, loanAccount.GLAccountId);
            await _context.SaveChangesAsync();


            return loanAccount;
        }

        public async Task EditLoanAccountAsync(LoanAccountViewModel accountViewModel)
        {
            var loanAccount = await RetrieveLoanAccountAsync(accountViewModel.GLAccountId);
            loanAccount.AccountName = accountViewModel.AccountName;
            loanAccount.DurationYears = accountViewModel.DurationYears;
            loanAccount.RepaymentFrequencyMonths = accountViewModel.RepaymentFrequencyMonths;
            var noPaymentsPerYear = 12 / loanAccount.RepaymentFrequencyMonths;
            loanAccount.CompoundInterest = (loanAccount.Principal * Math.Pow((1 + (loanAccount.InterestRate / 100) / (noPaymentsPerYear)), (noPaymentsPerYear * loanAccount.DurationYears))) - loanAccount.Principal;
            _context.Update(loanAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanAccount> RetrieveLoanAccountAsync(int id)
        {
            var account = await _context.LoanAccounts
                .Include(c => c.CustomerAccount)
                .FirstOrDefaultAsync(m => m.GLAccountId == id);
            return account;
        }

        public async Task<List<LoanAccount>> ListLoanAccountsAsync()
        {
            var accounts = _context.LoanAccounts.Include(l => l.CustomerAccount);
            return await accounts.ToListAsync();
        }

        public async Task<LoanAccountViewModel> GetAddLoanAccount(string linkedAccountNumber)
        {
            var linkedAccount = await _context.CustomerAccounts.Include(a=>a.Customer).Where(c => c.AccountNumber == linkedAccountNumber).FirstOrDefaultAsync();
            var customerAccounts = await _context.CustomerAccounts.Select(c => c.AccountNumber).ToListAsync();
            var interestRate = await _context.AccountConfigurations.Select(s => s.LoanInterestRate).FirstOrDefaultAsync();
            var viewModel = new LoanAccountViewModel
            {
                LinkedAccount = linkedAccount,
                CustomerAccounts = customerAccounts,
                InterestRate = (double)interestRate
            };
            return viewModel;
        }

        public async Task<LoanAccountViewModel> GetEditLoanAccount(int id)
        {
            var account = await RetrieveLoanAccountAsync(id);
            var viewModel = new LoanAccountViewModel
            {
                GLAccountId = account.GLAccountId,
                LinkedAccount=account.CustomerAccount,
                Principal = account.Principal,
                InterestRate = account.InterestRate,
                DurationYears = account.DurationYears,
                RepaymentFrequencyMonths=account.RepaymentFrequencyMonths,
                StartDate = account.StartDate,
                AccountNumber = account.AccountNumber
            };
            return viewModel;
        }
    }
}
