namespace CRM.ServiceCommon.Services.CodeService.Models
{
    public class GenerateCodeResult : CodeResult
    {
        public GenerateCodeError? Error { get; set; }
                public string Code { get; set; }
        
                public enum GenerateCodeError
                {
                    AlreadyCreated
                }
    }
}