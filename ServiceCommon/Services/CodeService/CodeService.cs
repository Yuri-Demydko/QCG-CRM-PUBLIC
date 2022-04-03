using System;
using System.Threading.Tasks;
using CRM.DAL;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.ServiceCommon.Services.CodeService.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.ServiceCommon.Services.CodeService
{
    public class CodeService : ICodeService
    {
        private readonly EmailCodeConfiguration configuration;
        private Action<DbContextOptionsBuilder> OptionsAction { get; }


        public CodeService(EmailCodeConfiguration configuration, Action<DbContextOptionsBuilder> optionsAction)
        {
            this.configuration = configuration;
            OptionsAction = optionsAction;
        }

        public async Task<GenerateCodeResult> GenerateCodeAsync(string email, VerifyCodeType verifyCodeType)
        {
            var builder = new DbContextOptionsBuilder<GeneralDbContext>();

            OptionsAction(builder);

            await using var dbContext = new GeneralDbContext(builder.Options);
            
            var code = GenerateCode();
            var result = new GenerateCodeResult();

            var emailVerifyCode = await dbContext.EmailVerifyCodes.FirstOrDefaultAsync(sc =>
                sc.Email == email && sc.VerifyCodeType == verifyCodeType);
            if (emailVerifyCode != null)
            {
                if (emailVerifyCode.CreatedAt.AddMinutes(configuration.CodeLifeTime.TotalMinutes) > DateTime.UtcNow.AddHours(3) && emailVerifyCode.TryCount<4)
	            {
		            result.Error = GenerateCodeResult.GenerateCodeError.AlreadyCreated;
                    result.Errors.Add($"Письмо с кодом подтверждения уже было отправлено. Отправить заново можно будет через {configuration.CodeLifeTime.TotalMinutes} минут");
                    return result;
                }

                emailVerifyCode.Code = code;
                emailVerifyCode.CreatedAt = DateTime.UtcNow.AddHours(3);
                emailVerifyCode.TryCount = 0;
            }
            else
            {
                await dbContext.EmailVerifyCodes.AddAsync(new EmailVerifyCode
                {
                    Email = email,
                    Code = code,
                    CreatedAt = DateTime.UtcNow.AddHours(3),
                    TryCount = 0,
                    VerifyCodeType = verifyCodeType
                });
            }

            await dbContext.SaveChangesAsync();

            result.Code = code;

            return result;
        }

        public async Task<ValidateCodeResult> ValidateCodeAsync(string email, VerifyCodeType verifyCodeType,
            string code)
        {
            var builder = new DbContextOptionsBuilder<GeneralDbContext>();

            OptionsAction(builder);

            await using var dbContext = new GeneralDbContext(builder.Options);
            
            var result = new ValidateCodeResult();

            var emailVerifyCode = await dbContext.EmailVerifyCodes.FirstOrDefaultAsync(sc =>
                sc.Email == email && sc.VerifyCodeType == verifyCodeType);
            if (emailVerifyCode == null)
            {
	            result.Error = ValidateCodeResult.ValidateCodeError.NoCodeFound;
                result.Errors.Add("На данный адрес не был отправлен код");
                return result;
            }

            if (!IsCodeAlive(emailVerifyCode.CreatedAt))
            {
	            result.Error = ValidateCodeResult.ValidateCodeError.CodeExpired;
                result.Errors.Add("Время жизни кода истекло");
                return result;
            }

            if (emailVerifyCode.TryCount >= 4)
            {
	            result.Error = ValidateCodeResult.ValidateCodeError.AttemptsCountExceeded;
	            result.Errors.Add("Превышено количество попыток ввода кода. Необходимо запросить новый");
	            return result;
            }

            if (emailVerifyCode.Code != code)
            {
	            result.Error = ValidateCodeResult.ValidateCodeError.IncorrectCode;
                emailVerifyCode.TryCount += 1;
                result.Errors.Add($"Некорректный код. Оставшееся количество попыток - {5 - emailVerifyCode.TryCount}");
            }

            await dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<CodeResult> IsCodeWasSent(string email, VerifyCodeType verifyCodeType)
        {
            var builder = new DbContextOptionsBuilder<GeneralDbContext>();

            OptionsAction(builder);

            await using var generalDbContext = new GeneralDbContext(builder.Options);
            
            var result = new ValidateCodeResult();

            var smsCode = await generalDbContext.EmailVerifyCodes.FirstOrDefaultAsync(sc =>
                sc.Email == email && sc.VerifyCodeType == verifyCodeType);

            if (smsCode == null)
            {
                var error = verifyCodeType switch
                {
                    VerifyCodeType.Registration => "На данный адрес не был отправлен код для регистрации",
                    VerifyCodeType.ForgotPassword =>
                        "На данный адрес не был отправлен код для восстановления доступа",
                    _ => "На данный адрес не был отправлен код"
                };

                result.Errors.Add(error);

                return result;
            }

            if (IsCodeAlive(smsCode.CreatedAt))
            {
                return result;
            }

            result.Errors.Add("Время жизни кода истекло");

            return result;
        }

        public static string GenerateCode()
        {
            return new Random().Next(0, 999999).ToString("D6");
        }

        private bool IsCodeAlive(DateTime codeCreatedAt)
        {
            var verifyCodeLifeTime = configuration.CodeLifeTime;

            return codeCreatedAt.Add(verifyCodeLifeTime) > DateTime.UtcNow.AddHours(3);
        }
    }
}