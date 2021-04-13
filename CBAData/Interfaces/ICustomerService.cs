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
        public Task<Customer> RetrieveCustomerAsync(int id);
        public Task DeleteCustomerAsync(int id);
        public Task<List<Customer>> ListCustomersAsync();
        public Task<bool> CustomerExists(int id);
    }
}
