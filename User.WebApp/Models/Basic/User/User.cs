using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.User
{
    public class User : DAL.Models.Users.User
    {
        public new ICollection<UserRole.UserRole> UserRoles { get; set; }

    }
}