using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.Models
{
    public class Posting
    {
        public int Id { get; set; }

        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Transaction ID")]
        public long? TransactionId { get; set; }
        public float? Debit { get; set; }
        public float? Credit { get; set; }
        public float Balance { get; set; }

        [DisplayName("Posting Date")]
        public DateTime PostingDate { get; set; }
        public string CBAUserId { get; set; }

        [DisplayName("Posted By")]
        public CBAUser PostedBy { get; set; }
        public string Notes { get; set; }
        public int GLAccountId { get; set; }
        public GLAccount GLAccount { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Code")]
        public string AccountCode { get; set; }
    }
}
