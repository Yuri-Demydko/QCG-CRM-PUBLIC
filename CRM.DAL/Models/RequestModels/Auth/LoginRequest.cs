using System.Collections.Generic;
using System.Linq;
using CRM.DAL.Models.ResponseModels.Error;

namespace CRM.DAL.Models.RequestModels.Auth
{
    public class LoginRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        public LoginRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public LoginRequest()
        {
        }

        public (bool, ErrorResponse) Validate()
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(Login))
            {
                errors.Add(new Error("Логин отсутствует", "login"));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add(new Error("Пароль отсутствует", "password"));
            }

            return (!errors.Any(), new ErrorResponse(errors));
        }
    }
}