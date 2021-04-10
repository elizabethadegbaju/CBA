using CBAData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CBAData.ViewModels
{
    public class CustomerAccountViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [EnumDataType(typeof(AccountClass)), DisplayName("Account Class")]
        public AccountClass AccountClass { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Account Status")]
        public bool IsActivated { get; set; }
    }
}
