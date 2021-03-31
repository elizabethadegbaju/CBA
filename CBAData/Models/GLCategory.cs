using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.Models
{
    public class GLCategory
    {
        public GLCategory()
        {
            IsEnabled = true;
        }

        public int Id { get; set; }
        public AccountType AccountType { get; set; }
        public string Name { get; set; }

        [DisplayName("Status")]
        public bool IsEnabled { get;set; }
    }
}
