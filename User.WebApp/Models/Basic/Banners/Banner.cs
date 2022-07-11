using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.User.WebApp.Models.Basic.Banners
{
	[Table("Banners")]
	public class Banner
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string ImagePath { get; set; }
		public string Text { get; set; }
		public string Title { get; set; }
		public string ButtonText { get; set; }

	}
	public class BannerConfiguration : IEntityTypeConfiguration<Banner>
	{
		public void Configure(EntityTypeBuilder<Banner> item)
		{
		}
	}
}
