using System;
using System.Collections.Generic;
using System.Text;

namespace CBAData.Models
{
    public enum StatusType
    {
        Error,
        Success,
        Info
    }

    public class StatusMessage
    {
        public StatusType Type { get; set; }
        public string Message { get; set; }
    }
}
