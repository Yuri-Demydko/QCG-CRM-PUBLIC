using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace CRM.ServiceCommon.Helpers
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RussianLanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = $"Произошла неизвестная ошибка." };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure), Description = "Ошибка параллельной обработки, объект был изменён"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(PasswordMismatch), Description = "Неверный пароль." };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError { Code = nameof(InvalidToken), Description = "Неверный токен." };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
                { Code = nameof(LoginAlreadyAssociated), Description = "Пользователь с таким логином уже существует." };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"Недопустимое имя пользователя '{userName}', имя может содержать только буквы или цифры."
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = $"Неправильный email '{email}'." };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
                { Code = nameof(DuplicateUserName), Description = $"Имя пользователя '{userName}' уже занято." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' уже занят." };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
                { Code = nameof(InvalidRoleName), Description = $"Недопустимое имя роли '{role}'." };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
                { Code = nameof(DuplicateRoleName), Description = $"Имя роли '{role}' уже занято." };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
                { Code = nameof(UserAlreadyHasPassword), Description = "Для пользователя уже назначен пароль." };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
                { Code = nameof(UserLockoutNotEnabled), Description = "Локаут для этого пользователя не активирован" };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
                { Code = nameof(UserAlreadyInRole), Description = $"У пользователя уже есть роль '{role}'." };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
                { Code = nameof(UserNotInRole), Description = $"У пользователя нет роли '{role}'." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort), Description = $"Пароль должен содержать минимум {length} символов."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Пароль должен содержать минимум один специальный символ (!,\"№;%:?*(_)+.)"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Пароль должен содержать минимум одну цифру ('0'-'9')."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Пароль должен содержать минимум один символ нижнего регистра ('a'-'z')."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Пароль должен содержать минимум один символ верхнего регистра ('A'-'Z')."
            };
        }
    }
}