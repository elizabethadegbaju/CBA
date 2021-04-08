﻿using System;
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

        public int GLAccountId { get; set; }

        public int GLCategoryId { get; set; }
        public GLCategory GLCategory { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        public CBAUser User { get; set; }
        public string CBAUserId { get; set; }

        [DisplayName("Account Status")]
        public bool IsActivated { get; set; }
    }
}
