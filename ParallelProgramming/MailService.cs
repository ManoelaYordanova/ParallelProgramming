using System.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ParallelProgramming
{
    public class MailService
    {
        public void Send(Emails text)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("anna.ivanovaaa25@gmail.com"));
            email.To.Add(MailboxAddress.Parse(text.Email));
            email.Subject = "Some subject";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = text.Text,
            };

            using SmtpClient client = new();
            client.Connect(MailManager.Gmail, 587, SecureSocketOptions.StartTls);
            client.Authenticate("anna.ivanovaaa25@gmail.com", "annaivanova25@");
            client.Send(email);
            client.Disconnect(true);
            text.Processed = true;
        }
    }
}
