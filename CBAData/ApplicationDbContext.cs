using CBAData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace CBAData
{
    public class ApplicationDbContext : IdentityDbContext<CBAUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        new public DbSet<CBARole> Roles { get; set; }
        public DbSet<GLCategory> GLCategories { get; set; }
        public DbSet<GLAccount> GLAccounts { get; set; }
        public DbSet<InternalAccount> InternalAccounts { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AccountConfiguration> AccountConfigurations { get; set; }
        public DbSet<Posting> Postings { get; set; }
        public DbSet<LoanAccount> LoanAccounts { get; set; }
    }
}
