using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.User.WebApp.Models.ViewModal
{
	public class ShopViewModal
	{
        public ICollection<Banners> Banners { get; set; }
        public ICollection<Product> BestProducts { get; set; }
        public ICollection<Product> Products { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
