using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
        // [Required(ErrorMessage = "Укажите телефон!")]
        // [Display(Name = "Телефон")]
        // [MaxLength(20, ErrorMessage = "Телефон не может быть больше 20 символов")]
        // public string Phone { get; set; }

        // [Required] public int Code { get; set; }
    }
}