using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.Role
{
    public class Role : DAL.Models.Users.Role
    {
        protected internal new ICollection<UserRole.UserRole> UserRoles { get; set; }
    }
}