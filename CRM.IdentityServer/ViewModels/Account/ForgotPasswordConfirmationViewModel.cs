using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class ForgotPasswordConfirmationViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [Display(Name="Код подтверждения")]
        public string Code { get; set; }
        
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string PasswordConfirm { get; set; }
    }
}