using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Models.DatabaseModels.SiaRenterAllowances
{
    public class SiaRenterAllowance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Period { get; set; }
        
        public string RenewWindow { get; set; }
        
        public string Funds { get; set; }
        
        public string Hosts { get; set; }
        
        public DateTime RegistrationTime { get; set; }
    }
}