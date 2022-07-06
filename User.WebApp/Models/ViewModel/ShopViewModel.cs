using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Banners;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.User.WebApp.Models.ViewModel
{
	public class ShopViewModel
	{
        public ICollection<Banner> Banners { get; set; }
        public ICollection<Product> BestProducts { get; set; }
        public ICollection<Product> Products { get; set; } 
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

		public ShopViewModel(ICollection<Banner> banners, ICollection<Product> bestProducts, ICollection<Product> products, int totalPages, int currentPage)
		{
			Banners = banners;
			BestProducts = bestProducts;
			Products = products;
			TotalPages = totalPages;
			CurrentPage = currentPage;
		}
	}
}
