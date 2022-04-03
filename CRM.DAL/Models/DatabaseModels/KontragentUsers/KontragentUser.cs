using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Kontragents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.KontragentUsers
{
    public class KontragentUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Kontragent Kontragent { get; set; }
        
        public Users.User User { get; set; }
        
        public Guid KontragentId { get; set; }
        
        public string UserId { get; set; }
        
        public KontragentUserRelationType RelationType { get; set; }
    }
    
    public class KontragentUserConfiguration : IEntityTypeConfiguration<KontragentUser>
    {
        public void Configure(EntityTypeBuilder<KontragentUser> item)
        {
            item.HasOne(i => i.Kontragent)
                .WithMany(r => r.KontragentUsers)
                .HasForeignKey(i => i.KontragentId);
            
            item.HasOne(i => i.User)
                .WithMany(r => r.KontragentUsers)
                .HasForeignKey(i => i.UserId);
        }
    }
}