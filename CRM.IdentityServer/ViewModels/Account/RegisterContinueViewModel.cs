using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class RegisterContinueViewModel
    {
        [Required]
        [MinLength(6)]
        [Display(Name="Код подтверждения")]
        public string Code { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
    }
}