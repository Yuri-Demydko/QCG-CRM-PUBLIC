using System.ComponentModel.DataAnnotations;

namespace Common.Enum
{
	public enum SortType
	{
		[Display(Name = "По убыванию цены")]
		PriceDescending = 0,
		[Display(Name = "По возрастанию цены")]
		PriceAscending = 1,
		[Display(Name = "Сначала популярные")]
		PopularFirst = 2,
	}
}
