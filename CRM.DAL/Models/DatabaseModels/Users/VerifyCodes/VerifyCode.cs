using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;

namespace CRM.DAL.Models.DatabaseModels.Users.VerifyCodes
{
    [Table("VerifyCodes")]
    public class VerifyCode
    {
        [Key] public Guid Id { get; set; }

        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsVerify { get; set; }

        public int TryCount { get; set; }

        public VerifyCodeType VerifyCodeType { get; set; }
    }
}