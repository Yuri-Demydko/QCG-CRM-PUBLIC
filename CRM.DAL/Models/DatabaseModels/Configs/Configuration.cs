using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Models.DatabaseModels.Configs
{
    [Table("CommonConfigs")]
    public class Configuration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Value { get; set; }
    }
}