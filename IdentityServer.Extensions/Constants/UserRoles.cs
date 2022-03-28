using System.Collections.Generic;

namespace CRM.IdentityServer.Extensions.Constants
{
    public static class UserRoles
    {
        public const string Admin = "admin";

        public const string User = "user";
        public const string DraftUser = "draftuser";

        public static Dictionary<string, string> RoleNames = new Dictionary<string, string>()
        {
            [Admin] = "Администратор",
            [User] = "Логист",
            [DraftUser] = "Пользователь во время регистрации",
        };
    }
}