using CBAData;
using CBAData.Interfaces;
using CBAData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAService
{
    public class AccountConfigurationService : IAccountConfigurationService
    {
        public readonly ApplicationDbContext _context;

        public AccountConfigurationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ClearAccountConfiguration()
        {
            var accountConfiguration = await RetrieveAccountConfiguration();
            accountConfiguration.SavingsInterestRate = null;
            accountConfiguration.LoanInterestRate = null;
            accountConfiguration.SavingsMinBalance = null;
            accountConfiguration.CurrentMinBalance = null;
            accountConfiguration.SavingsMaxDailyWithdrawal = null;
            accountConfiguration.CurrentMaxDailyWithdrawal = null;
            _context.AccountConfigurations.Update(accountConfiguration);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountConfiguration> RetrieveAccountConfiguration()
        {
            return await _context.AccountConfigurations.FirstOrDefaultAsync();
        }

        public Task<bool> IsAccountConfigurationComplete()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAccountConfiguration(AccountConfiguration accountConfiguration)
        {
            if (accountConfiguration.Id == 0)
            {
                accountConfiguration.FinancialDate = DateTime.Now;
                _context.Add(accountConfiguration);
                await _context.SaveChangesAsync();
                return;
            }
            _context.Update(accountConfiguration);
            await _context.SaveChangesAsync();
        }
    }
}
