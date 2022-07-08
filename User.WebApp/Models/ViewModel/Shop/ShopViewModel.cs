using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enum;
using CRM.DAL.Models.DatabaseModels.Banners;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.Tags;
using Microsoft.AspNet.OData.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.User.WebApp.Models.ViewModel.Shop
{
	public class Shop
	{
		public Shop()
		{
			Banners = new HashSet<Banner>();
			BestProducts = new HashSet<Product>();
			Products = new HashSet<Product>();
		}
		[Key]
		public int Id { get; set; }//for odata only
		[AutoExpand]
		public ICollection<Banner> Banners { get; set; }
		[AutoExpand]
		public ICollection<Product> BestProducts { get; set; }
		[AutoExpand]
		public ICollection<Product> Products { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }

		public Shop(ICollection<Banner> banners, ICollection<Product> bestProducts, ICollection<Product> products, int totalPages, int currentPage)
		{
			Id = 0;
			Banners = banners;
			BestProducts = bestProducts;
			Products = products;
			TotalPages = totalPages;
			CurrentPage = currentPage;
		}
	}
}
