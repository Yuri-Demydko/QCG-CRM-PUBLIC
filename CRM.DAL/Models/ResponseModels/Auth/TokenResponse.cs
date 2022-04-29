namespace CRM.DAL.Models.ResponseModels.Auth
{
    public class TokenResponse
    {
        /// <summary>
        /// Токен аутентификации
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; }

        public TokenResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}