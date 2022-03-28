using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class EmailForgotPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult(
                    "Укажите Email для восстановления",
                    new[] { "Email" });
            }
        }
    }
}