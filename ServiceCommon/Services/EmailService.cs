using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

#pragma warning disable 618

namespace CRM.ServiceCommon.Services
{
    public class EmailConfiguration
    {
        public readonly ICredentials Credentials;
        public readonly string Host;
        public readonly int Port;
        public readonly bool EnableSsl;

        public EmailConfiguration(string host, int port, ICredentials credentials, bool enableSsl)
        {
            Host = host;
            Port = port;
            Credentials = credentials;
            EnableSsl = enableSsl;
        }
    }

    public interface IEmailService
    {
        public Task SendEmailAsync(IEnumerable<string> addresses, string subject, string textBody, string htmlBody,
            IEnumerable<MimePart> attachments = null,
            IEnumerable<string> bccs = null, TextFormat textFormat = TextFormat.Html);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration configuration;
        private readonly MailboxAddress fromAddress;

        public EmailService(EmailConfiguration configuration, MailboxAddress fromAddress)
        {
            this.configuration = configuration;
            this.fromAddress = fromAddress;
        }

        public async Task SendEmailAsync(IEnumerable<string> addresses, string subject, string textBody,
            string htmlBody,
            IEnumerable<MimePart> attachments = null,
            IEnumerable<string> bccs = null, TextFormat textFormat = TextFormat.Html)
        {
            var message = new MimeMessage
            {
                Subject = subject,
            };

            var builder = new BodyBuilder
            {
                TextBody = textBody,
                HtmlBody = htmlBody,
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment);
                }
            }

            message.Body = builder.ToMessageBody();

            if (bccs != null)
            {
                foreach (var bcc in bccs)
                {
                    message.Bcc.Add(new MailboxAddress(bcc));
                }
            }

            message.From.Add(fromAddress);

            foreach (var address in addresses)
            {
                message.To.Add(new MailboxAddress(address));
            }

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            await client.ConnectAsync(configuration.Host, configuration.Port, configuration.EnableSsl);

            await client.AuthenticateAsync(configuration.Credentials);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
    
}