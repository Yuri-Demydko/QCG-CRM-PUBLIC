using System.Net;
using CRM.ServiceCommon.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace CRM.ServiceCommon.Configurations
{
    public static class EmailSenderConfiguration
    {
        public static void ConfigureEmail(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IEmailService>(provider =>
                new EmailService(new EmailConfiguration(
                    configuration.GetValue<string>("emailsmtpclient"),
                    configuration.GetValue<int>("emailsmtpport"),
                    new NetworkCredential(configuration.GetValue<string>("emailsmtpuser"),
                        configuration.GetValue<string>("emailsmtpuserpassword")),
                    false), new MailboxAddress(configuration.GetValue<string>("emailsmtpfromaddress"))
                )
            );
        }

    }
}