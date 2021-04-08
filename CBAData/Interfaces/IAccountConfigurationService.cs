using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface IAccountConfigurationService
    {
        public Task UpdateAccountConfiguration(AccountConfiguration accountConfiguration);
        public Task<AccountConfiguration> RetrieveAccountConfiguration();
        public Task<bool> IsAccountConfigurationComplete();
        public Task ClearAccountConfiguration();
    }
}
