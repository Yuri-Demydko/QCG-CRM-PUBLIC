namespace CRM.ServiceCommon.Services.CodeService.Models
{
    public class ValidateCodeResult : CodeResult
    {
        public ValidateCodeError? Error { get; set; }

        public enum ValidateCodeError
        {
            NoCodeFound,
            CodeExpired,
            AttemptsCountExceeded,
            IncorrectCode,
        }
    }
}