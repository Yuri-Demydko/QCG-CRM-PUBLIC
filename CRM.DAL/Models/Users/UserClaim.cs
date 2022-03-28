using Microsoft.AspNetCore.Identity;

namespace CRM.DAL.Models.Users
{
    public class UserClaim : IdentityUserClaim<string>
    {
        protected internal User User { get; set; }
    }
}