using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.Models
{
    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Reciever { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public static MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content
            };
            return mimeMessage;
        }
    }
}
