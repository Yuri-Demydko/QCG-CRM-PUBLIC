using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Models.DatabaseModels.SiaMonitoredBlock
{
    public class SiaMonitoredBlock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public long Height { get; set; }
        
        public string Hash { get; set; }
        
        public DateTime MonitoringTime { get; set; }
    }
}