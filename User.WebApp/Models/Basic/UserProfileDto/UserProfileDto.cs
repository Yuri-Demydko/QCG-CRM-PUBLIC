using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.UserProfileDto
{
    public class UserProfileDto : DAL.Models.DatabaseModels.Users.User
    {
        public List<string> Roles { get; set; }
    }
}