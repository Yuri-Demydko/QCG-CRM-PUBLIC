using System.ComponentModel.DataAnnotations;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class PersonalAreaViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        public bool EmailConfirmed { get; set; }
    }
}