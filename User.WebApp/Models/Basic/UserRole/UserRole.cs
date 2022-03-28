namespace CRM.User.WebApp.Models.Basic.UserRole
{
    public class UserRole : DAL.Models.Users.UserRole
    {
        public new virtual User.User User { get; set; }

        public new virtual Role.Role Role { get; set; }
    }
}