using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CBAData.Models
{
    public class CustomerAccount : GLAccount
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [EnumDataType(typeof(AccountClass)), DisplayName("Account Class")]
        public AccountClass AccountClass { get; set; }

        [DisplayName("Date Opened")]
        public DateTime DateOpened { get; set; }

        [DisplayName("Loan Account")]
        public LoanAccount LoanAccount { get; set; }
    }
}
