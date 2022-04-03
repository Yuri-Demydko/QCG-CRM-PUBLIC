using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.IdentityServer.Models;
using CRM.ServiceCommon.Constants;
using CRM.ServiceCommon.Services;
using Hangfire;
using Microsoft.Extensions.Logging;
using MimeKit;
using RazorLight;
using ILogger = Amazon.Runtime.Internal.Util.ILogger;

namespace CRM.IdentityServer.Services
{
    [Queue("identity")]
    public class EmailSenderService
    {
        private readonly IEmailService emailService;
        private readonly RazorLightEngine engine;
        private readonly IdentityServerDbContext identityDbContext;
        private readonly ILogger<EmailSenderService> logger;

        public EmailSenderService(RazorLightEngine engine, IdentityServerDbContext identityDbContext, IEmailService emailService, ILogger<EmailSenderService> logger)
        {
            this.engine = engine;
            this.identityDbContext = identityDbContext;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task SendTestEmail()
        {
            var addressesTo = new List<string>
            {
                "yuridemydko@gmail.com",
            };
                 List<MimePart> mimePart = new List<MimePart>();
                 logger.LogInformation("Trying to send test email");
                 try
                 {
                     await emailService.SendEmailAsync(addressesTo, "SMTP-test", "SMTP Service test", null, mimePart);
                 }
                 catch (Exception e)
                 {
                     logger.LogError($"Send failed. Error:{e.Message}");
                 }
        }

        public async Task SendVerifyCodeEmail(string email, string code, VerifyCodeType codeType)
        {
            var addressesTo = new List<string>()
            {
                email
            };
            List<MimePart> mimePart = new List<MimePart>();
            var text = "";
            var subj = "";
            switch (codeType)
            {
                case VerifyCodeType.Registration:
                    text = $"Код для регистрации: {code}";
                    subj = EmailSubjects.Registration;
                    break;
                case VerifyCodeType.ForgotPassword:
                    text = $"Код для сброса пароля: {code}";
                    subj = EmailSubjects.ResetPassword;
                    break;
            }
            logger.LogInformation($"Trying to send code: {code}");
            try
            {
                await emailService.SendEmailAsync(addressesTo, subj, text, null, mimePart);
            }
            catch (Exception e)
            {
                logger.LogError($"Code wasn't send due too SMTP Error {e.Message}");
            }
            logger.LogInformation("Send code - OK");
        }
        
    }
}