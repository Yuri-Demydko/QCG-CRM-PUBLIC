using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.Users.VerifyCodes.Enums;
using CRM.IdentityServer.Models;
using CRM.ServiceCommon.Constants;
using CRM.ServiceCommon.Services;
using CRM.ServiceCommon.Services.CodeService.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MimeKit;
using RazorLight;
using Z.EntityFramework.Plus;

namespace CRM.IdentityServer.Services
{
    [Queue("identity")]
    public class EmailSenderService
    {
        private readonly IEmailService emailService;
        private readonly RazorLightEngine engine;
        private readonly IdentityServerDbContext identityDbContext;

        public EmailSenderService(RazorLightEngine engine, IdentityServerDbContext identityDbContext, IEmailService emailService )
        {
            this.engine = engine;
            this.identityDbContext = identityDbContext;
            this.emailService = emailService;
        }

        public async Task SendTestEmail()
        {
            var addressesTo = new List<string>
            {
                "yuridemydko@gmail.com",
            };
                 List<MimePart> mimePart = new List<MimePart>();
                 await emailService.SendEmailAsync(addressesTo, "SMTP-test", "SMTP Service test", null, mimePart);
        }

        public async Task SendVerifyCodeEmail(string email, string code, VerifyCodeType codeType)
        {
            var addressesTo = new List<string>()
            {
                email
            };
            List<MimePart> mimePart = new List<MimePart>();
            var text = "";
            switch (codeType)
            {
                case VerifyCodeType.Registration:
                    text = $"Код для регистрации: {code}";
                    break;
                case VerifyCodeType.ForgotPassword:
                    text = $"Код для сброса пароля: {code}";
                    break;
            }
            await emailService.SendEmailAsync(addressesTo, EmailSubjects.Registration,text, null, mimePart);
        }
        
    }
}