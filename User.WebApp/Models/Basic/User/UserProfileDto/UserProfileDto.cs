using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.User.UserProfileDto
{
    public class UserProfileDto : User
    {
        public List<string> Roles { get; set; }
        public new string FullName { get; set; }
 
    }
}