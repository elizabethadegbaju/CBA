using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.Models
{
    public class InternalAccount : GLAccount
    {
        public CBAUser User { get; set; }
        public string CBAUserId { get; set; }

        [DisplayName("Account Code")]
        public string AccountCode { get; set; }
    }
}
