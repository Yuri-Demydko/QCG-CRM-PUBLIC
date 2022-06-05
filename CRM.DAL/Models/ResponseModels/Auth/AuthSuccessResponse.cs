using CRM.DAL.Models.DatabaseModels.Users;

namespace CRM.DAL.Models.ResponseModels.Auth
{
    public class AuthSuccessResponse
    {
        public TokenResponse Tokens { get; set; }
        
        public UserAuthResponseModel User { get; set; }
    }
}