using System;
using System.Collections.Generic;

namespace CRM.DAL.Models.DatabaseModels.ViewModel
{
	public class ShopViewModel
	{
		public int Id { get; set; }
		public ICollection<CRM.DAL.Models.DatabaseModels.Banners.Banner> Banners { get; set; }
		public ICollection<CRM.DAL.Models.DatabaseModels.Products.Product> BestProducts { get; set; }
		public ICollection<CRM.DAL.Models.DatabaseModels.Products.Product> Products { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }

		public ShopViewModel(ICollection<CRM.DAL.Models.DatabaseModels.Banners.Banner> banners, 
			ICollection<CRM.DAL.Models.DatabaseModels.Products.Product> bestProducts,
			ICollection<CRM.DAL.Models.DatabaseModels.Products.Product> products, int totalPages, int currentPage)
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