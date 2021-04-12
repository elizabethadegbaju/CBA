using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.ViewModels
{
    public class GLPostingViewModel
    {
        public float Amount { get; set; }
        public string Notes { get; set; }

        [DisplayName("Debit Account Code")]
        public string DebitAccountCode { get; set; }

        [DisplayName("Credit Account Code")]
        public string CreditAccountCode { get; set; }
    }
}
