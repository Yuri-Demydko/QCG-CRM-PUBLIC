using System;

namespace CRM.DAL.Models.ResponseModels.Auth
{
    public class TokenResponse
    {
        /// <summary>
        /// Токен аутентификации
        /// </summary>
        public string Token { get; }
        
        public DateTime TokenExpireDateTime { get; }

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; }

        public TokenResponse(string token, string refreshToken, DateTime tokenExpireDateTime)
        {
            Token = token;
            RefreshToken = refreshToken;
            TokenExpireDateTime = tokenExpireDateTime;
        }
    }
}