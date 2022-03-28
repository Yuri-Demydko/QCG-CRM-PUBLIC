using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class EmailResetPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string PasswordConfirm { get; set; }

        public string Code { get; set; }
    }
}