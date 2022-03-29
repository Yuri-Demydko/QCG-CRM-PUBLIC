using Microsoft.AspNetCore.Identity;

namespace CRM.DAL.Models.Users
{
    public class UserRole : IdentityUserRole<string>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}