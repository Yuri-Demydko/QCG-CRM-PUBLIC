using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class BaseAccessRecoveryViewModel
    {
        public string SelectedAccessRecoveryMethod { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(SelectedAccessRecoveryMethod))
            {
                yield return new ValidationResult(
                    "Выберите метод восстановления доступа",
                    new[] { "SelectedAccessRecoveryMethod" });
            }
        }
    }
}