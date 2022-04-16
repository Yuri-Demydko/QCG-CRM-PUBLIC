using System.ComponentModel.DataAnnotations;

namespace CRM.DAL.Models.RequestModels.ChangePassword
{
    public class ChangePasswordRequest
    {
        //Necessary for OData
        [Key]
        public int Id { get; set; }
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
    }
}