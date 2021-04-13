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
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CustomerExists(int id)
        {
            return await _context.Customers.AnyAsync(e => e.CustomerId == id);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await RetrieveCustomerAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task EditCustomerAsync(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> ListCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }

        public async Task<Customer> RetrieveCustomerAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            return customer;
        }
    }
}
