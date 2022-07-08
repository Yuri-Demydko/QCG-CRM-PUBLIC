using CRM.DAL.Models.DatabaseModels.Tags;
using System.Collections.Generic;
using Common.Enum;

namespace CRM.DAL.Models.RequestModels.Shop 
{ 
	public class ShopRequest
	{
		public int? CurrentPage { get; set; }
		public ICollection<Tag> Genres { get; set; }
		public SortType SortType { get; set; }
		public FilterByPrice? FilterByPrice { get; set; }

	}
}
