using System.Diagnostics.CodeAnalysis;

namespace CRM.DAL.Models.RequestModels.ChangeEmail
{
    public class ChangeEmailConfirmationRequest
    {
        [NotNull]
        public string Code { get; set; }
    }
}