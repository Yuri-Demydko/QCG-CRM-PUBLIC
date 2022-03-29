using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.User.UserProfileDto
{
    public class UserProfileDto : DAL.Models.Users.User
    {
        public List<string> Roles { get; set; }
    }
}