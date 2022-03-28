using System;

namespace CRM.IdentityServer.ViewModels.Account
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool AutomaticRedirectAfterSignOut = false;

        public static string InvalidCredentialsErrorMessage = "Некорректный логин или пароль";
    }
}