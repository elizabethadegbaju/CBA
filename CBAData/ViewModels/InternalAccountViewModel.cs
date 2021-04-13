using CBAData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CBAData.ViewModels
{
    public class InternalAccountViewModel
    {
        public InternalAccountViewModel()
        {
            GLCategories = new List<SelectListItem>();
            Users = new List<SelectListItem>();
        }

        [DisplayName("Account Number")]
        public string AccountCode { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Account Status")]
        public bool IsActivated { get; set; }

        public string CategoryId { get; set; }
        public IList<SelectListItem> GLCategories { get; set; }
        public IList<SelectListItem> Users { get; set; }
        public int Id { get; set; }
    }
}
