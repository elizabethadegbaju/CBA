using CBAData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.ViewModels
{
    public class TellerPostingViewModel
    {
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Transaction Slip Number")]
        public long? TransactionSlipNo { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

        [DisplayName("Transaction Type")]
        public TransactionType TransactionType { get; set; }
    }
}
