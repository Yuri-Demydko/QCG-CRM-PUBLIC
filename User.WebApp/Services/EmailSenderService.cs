using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.ServiceCommon.Constants;
using CRM.ServiceCommon.Services;
using CRM.User.WebApp.Models.Basic;
using Hangfire;
using Microsoft.Extensions.Logging;
using MimeKit;
using RazorLight;

namespace CRM.User.WebApp.Services
{
    [Queue("digital")]
    public class EmailSenderService
    {
        private readonly IEmailService emailService;
        private readonly RazorLightEngine engine;
        private readonly UserDbContext userDbContext;
        private readonly ILogger<EmailSenderService> logger;

        public EmailSenderService(RazorLightEngine engine, UserDbContext userDbContext, IEmailService emailService, ILogger<EmailSenderService> logger)
        {
            this.engine = engine;
            this.userDbContext = userDbContext;
            this.emailService = emailService;
            this.logger = logger;
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
                case VerifyCodeType.ChangeEmail:
                    text = $"Код для смены email адреса: {code}";
                    subj = EmailSubjects.Registration;
                    break;
            }
            logger.LogInformation($"Trying to send code: {code}");
            try
            {
                await emailService.SendEmailAsync(addressesTo, subj, text, null, mimePart);
            }
            catch (Exception e)
            {
                logger.LogError($"Code wasn't send due to SMTP Error {e.Message}");
            }
            logger.LogInformation("Send code - OK");
        }
        
    }
}