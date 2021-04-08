using System;
using System.Collections.Generic;
using System.Text;

namespace CBAData.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CustomerId { get; set; }
        public bool IsActivated { get; set; }
        public List<CustomerAccount> Accounts { get; set; }
    }
}
