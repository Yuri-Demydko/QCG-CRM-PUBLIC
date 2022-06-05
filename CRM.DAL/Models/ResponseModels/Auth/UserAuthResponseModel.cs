using System;
using System.Collections.Generic;
using CRM.DAL.Models.DatabaseModels.Users;

namespace CRM.DAL.Models.ResponseModels.Auth
{
    public class UserAuthResponseModel
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime RegistrationDate { get; set; }
        
        public IEnumerable<string> Roles { get; set; }
        
        public string Id { get; set; }
        
        public bool EmailConfirmed { get; set; }
        
        public string LastSiaAddress { get; set; }
        
        public decimal SiaCoinBalance { get; set; }
    }
}