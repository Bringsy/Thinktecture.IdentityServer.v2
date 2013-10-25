using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using BrockAllen.MembershipReboot;

namespace Thinktecture.IdentityServer.Web.Areas.Account
{
    public class SmtpHtmlMessageDelivery : IMessageDelivery
    {
        public void Send(Message msg)
        {
            if (String.IsNullOrWhiteSpace(msg.From))
            {
                var smtp = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                msg.From = smtp.From;
            }

            using (var smtp = new SmtpClient())
            {
                smtp.Send(new MailMessage(msg.From, msg.To, msg.Subject, msg.Body)
                {
                    IsBodyHtml = true
                });
            }
        }
    }
}