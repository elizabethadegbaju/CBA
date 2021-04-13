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

        [DisplayName("S/N")]
        public int GLCategoryId { get; set; }

        [EnumDataType(typeof(AccountType))]
        public AccountType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Status")]
        public bool IsEnabled { get;set; }

        [Required]
        public string Description { get; set; }

        public List<GLAccount> GLAccounts { get; set; }
    }
}
