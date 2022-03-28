using System;
using CRM.User.WebApp.Models.Basic.User.UserStaticDto.Enums;

namespace CRM.User.WebApp.Models.Basic.User.UserStaticDto
{
    public class UserStaticDto
    {
        public bool Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public UserStatisticTypes Type { get; set; }
    }
}