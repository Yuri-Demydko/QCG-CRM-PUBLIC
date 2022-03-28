using Microsoft.AspNetCore.Identity;

namespace CRM.DAL.Models.Users
{
    public class UserRole : IdentityUserRole<string>
    {
        protected internal User User { get; set; }

        protected internal Role Role { get; set; }
    }
}