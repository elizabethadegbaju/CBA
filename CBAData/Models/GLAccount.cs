using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.Models
{
    public class GLAccount
    {
        public GLAccount()
        {
            IsActivated = true;
        }

        public int Id { get; set; }

        [DisplayName("GL Category")]
        public GLCategory Category { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        public CBAUser User { get; set; }

        [DisplayName("Account Status")]
        public bool IsActivated { get; set; }
    }
}
