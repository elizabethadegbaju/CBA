using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.Models
{
    public class CustomerAccount : GLAccount
    {
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public AccountClass AccountClass { get; set; }
    }
}
