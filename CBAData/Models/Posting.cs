using System;
using System.Collections.Generic;
using System.Text;

namespace CBAData.Models
{
    public class Posting
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionId { get; set; }
        public float? Debit { get; set; }
        public float? Credit { get; set; }
        public float Balance { get; set; }
        public DateTime PostingDate { get; set; }
        public string CBAUserId { get; set; }
        public CBAUser PostedBy { get; set; }
        public string Notes { get; set; }
        public int GLAccountId { get; set; }
        public GLAccount GLAccount { get; set; }
        public string AccountNumber { get; set; }
        public string AccountCode { get; set; }
    }
}
