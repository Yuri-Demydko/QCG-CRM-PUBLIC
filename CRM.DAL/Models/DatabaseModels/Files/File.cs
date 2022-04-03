using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.Files
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OriginalName { get; set; }

        public string ContentType { get; set; }

        public string Path { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
        
        public FileType Type { get; set; }
        
        public ICollection<ProductFile.ProductFile> ProductFiles { get; set; }
        
        
    }
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> item)
        {
            item.HasMany(i => i.ProductFiles)
                .WithOne(i => i.File)
                .HasForeignKey(i => i.FileId);
            
        }
    }
}