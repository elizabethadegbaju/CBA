using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.ViewModels
{
    public class GLPostingViewModel
    {
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Transaction Id")]
        public long TransactionId { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }

        [DisplayName("Debit Account Code")]
        public string DebitAccountCode { get; set; }

        [DisplayName("Credit Account Code")]
        public string CreditAccountCode { get; set; }
    }
}
