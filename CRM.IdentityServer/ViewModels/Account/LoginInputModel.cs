using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}