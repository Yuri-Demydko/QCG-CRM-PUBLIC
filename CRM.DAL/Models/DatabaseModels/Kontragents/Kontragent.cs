using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Files;
using CRM.DAL.Models.DatabaseModels.KontragentUsers;
using CRM.DAL.Models.DatabaseModels.ProductsKontragents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.Kontragents
{
    public class Kontragent
    {
        //Developer/Publisher
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
      
        public string Title { get; set; }
        
        public File? Icon { get; set; }
        
        public Guid? IconId { get; set; }
        
        public ICollection<KontragentInfo.KontragentInfo> KontragentInfo { get; set; }//social media links etc
        
        public ICollection<KontragentUser> KontragentUsers { get; set; }
        
        public ICollection<ProductKontragent> ProductKontragents { get; set; }
    }
    
    public class KontragentConfiguration : IEntityTypeConfiguration<Kontragent>
    {
        public void Configure(EntityTypeBuilder<Kontragent> item)
        {
            item.HasMany(i => i.KontragentUsers)
                .WithOne(r => r.Kontragent)
                .HasForeignKey(r => r.KontragentId);
            
            item.HasMany(i => i.ProductKontragents)
                .WithOne(r => r.Kontragent)
                .HasForeignKey(r => r.KontragentId);
            
            item.HasMany(i => i.KontragentInfo)
                .WithOne(r => r.Kontragent)
                .HasForeignKey(r => r.KontragentId);

        }
    }
}