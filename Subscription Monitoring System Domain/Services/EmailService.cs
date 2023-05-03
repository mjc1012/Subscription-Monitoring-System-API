using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }

        public void SendEmail(EmailDto email)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmailConstants.Username, EmailConstants.From));
            emailMessage.To.Add(new MailboxAddress(email.To, email.To));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content)
            };

            using var client = new SmtpClient();
            try
            {
                client.Connect(EmailConstants.SmtpServer, EmailConstants.Port, EmailConstants.UseSsl);
                client.Authenticate(EmailConstants.From, EmailConstants.Password);
                client.Send(emailMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client.Disconnect(EmailConstants.Quit);
                client.Dispose();
            }
        }
    }
}
