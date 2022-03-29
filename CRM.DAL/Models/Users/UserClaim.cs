using Microsoft.AspNetCore.Identity;

namespace CRM.DAL.Models.Users
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public User User { get; set; }
    }
}