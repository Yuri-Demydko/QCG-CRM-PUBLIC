using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Kontragents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.KontragentInfo
{
    public class KontragentInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid KontragentId { get; set; }
        
        public Kontragent Kontragent { get; set; }
        
        public string InfoKey { get; set; }
        
        public string InfoValue { get; set; }
    }
    
    public class KontragentInfoConfiguration : IEntityTypeConfiguration<KontragentInfo>
    {
        public void Configure(EntityTypeBuilder<KontragentInfo> item)
        {
            item.HasOne(r => r.Kontragent)
                .WithMany(r => r.KontragentInfo)
                .HasForeignKey(r => r.KontragentId);
        }
    }
}