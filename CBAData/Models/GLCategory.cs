using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CBAData.Models
{
    public class GLCategory
    {
        public GLCategory()
        {
            IsEnabled = true;
        }

        public int GLCategoryId { get; set; }

        [EnumDataType(typeof(AccountType))]
        public AccountType Type { get; set; }
        public string Name { get; set; }

        [DisplayName("Status")]
        public bool IsEnabled { get;set; }

        public List<GLAccount> GLAccounts { get; set; }
    }
}
