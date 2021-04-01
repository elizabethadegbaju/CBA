using System;
using System.Collections.Generic;
using System.Text;

namespace CBAData.Models
{
    public class GLAccount
    {
        public int Id { get; set; }
        public GLCategory Category { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public CBAUser User { get; set; }
        public bool IsActivated { get; set; }
    }
}
