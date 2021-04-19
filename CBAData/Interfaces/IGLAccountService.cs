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
        public Task AddInternalAccountAsync(InternalAccountViewModel accountViewModel);
        public Task EditInternalAccountAsync(InternalAccountViewModel accountViewModel);
        public Task<InternalAccount> RetrieveInternalAccountAsync(int id);
        public Task<List<InternalAccount>> ListInternalAccountsAsync();
        public InternalAccountViewModel GetAddInternalAccount();
        public Task<InternalAccountViewModel> GetEditInternalAccount(int id);

        public Task<CustomerAccount> AddCustomerAccountAsync(CustomerAccountViewModel accountViewModel);
        public Task EditCustomerAccountAsync(CustomerAccountViewModel accountViewModel);
        public Task<CustomerAccount> RetrieveCustomerAccountAsync(int id);
        public Task<List<CustomerAccount>> ListCustomerAccountsAsync();
        public CustomerAccountViewModel GetAddCustomerAccount(int customerId, AccountClass accountClass);
        public Task<CustomerAccountViewModel> GetEditCustomerAccount(int id);

        public Task<LoanAccount> AddLoanAccountAsync(LoanAccountViewModel accountViewModel);
        public Task EditLoanAccountAsync(LoanAccountViewModel accountViewModel);
        public Task<LoanAccount> RetrieveLoanAccountAsync(int id);
        public Task<List<LoanAccount>> ListLoanAccountsAsync();
        public Task<LoanAccountViewModel> GetAddLoanAccount(string linkedAccountNumber);
        public Task<LoanAccountViewModel> GetEditLoanAccount(int id);

        public Task DeleteGLAccountAsync(int id);
        public Task ActivateGLAccountAsync(int id);
        public Task DeactivateGLAccountAsync(int id);
        public Task<bool> GLAccountExists(int id);
    }
}
