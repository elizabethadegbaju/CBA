using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBA.Models
{
    public class EmailMetadata
    {
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
