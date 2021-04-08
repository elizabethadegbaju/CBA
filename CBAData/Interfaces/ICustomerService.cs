using CBAData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBAData.Interfaces
{
    public interface ICustomerService
    {
        public Task AddCustomerAsync(Customer customer);
        public Task EditCustomerAsync(Customer customer);
        public Task<Customer> RetrieveCustomerAsync(long id);
        public Task DeleteCustomerAsync(long id);
        public Task<List<Customer>> ListCustomersAsync();
        public Task<bool> CustomerExists(long id);
    }
}
