using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.ServiceCommon.Services.CodeService.Models;

namespace CRM.ServiceCommon.Services.CodeService
{
    public interface ICodeService
    {
        Task<GenerateCodeResult> GenerateCodeAsync(string email, VerifyCodeType verifyCodeType);

        Task<ValidateCodeResult> ValidateCodeAsync(string email, VerifyCodeType verifyCodeType, string code);

        Task<CodeResult> IsCodeWasSent(string email, VerifyCodeType verifyCodeType);
    }
}