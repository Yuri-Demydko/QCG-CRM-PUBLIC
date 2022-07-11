using System.Collections.Generic;
using Common.Enum;
using CRM.DAL.Models.DatabaseModels.Tags;

namespace CRM.User.WebApp.Models.Request.Shop 
{ 
	public class ShopRequest
	{
		public int? CurrentPage { get; set; }
		public ICollection<Tag> Tag { get; set; }
		public SortType SortType { get; set; }
		public FilterByPrice? FilterByPrice { get; set; }

	}
}
