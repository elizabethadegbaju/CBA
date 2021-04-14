using System;
using System.Collections.Generic;
using System.ComponentModel;
using CBAData.Models;

namespace CBAData.ViewModels
{
    public class LoanAccountViewModel
    {
        public LoanAccountViewModel()
        {
        }

        public int GLAccountId { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Linked Account")]
        public CustomerAccount LinkedAccount { get; set; }
        public double Principal { get; set; }

        [DisplayName("% Interest Rate")]
        public double InterestRate { get; set; }

        [DisplayName("Duration in Years")]
        public float DurationYears { get; set; }

        [DisplayName("Repayment Frequency in Months")]
        public float RepaymentFrequencyMonths { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        public List<string> CustomerAccounts { get; set; }
    }
}
