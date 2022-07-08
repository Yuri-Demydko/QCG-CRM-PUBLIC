using Common.Attribute;
using System.ComponentModel.DataAnnotations;

namespace Common.Enum
{
	public enum FilterByPrice
	{
		[FilterByPrice(0, false)]
		[Display(Name="Бесплатно")]
		Free = 0,

		[FilterByPrice(500, false)]
		[Display(Name = "До 500")]
		Before500 = 1,

		[FilterByPrice(1000, false)]
		[Display(Name = "До 1000")]
		Before100 = 2,

		[FilterByPrice(1500, false)]
		[Display(Name = "До 1500")]
		Before1500 = 3,

		[FilterByPrice(2000, false)]
		[Display(Name = "До 2000")]
		Before2000 = 4,

		[FilterByPrice(1000, true)]
		[Display(Name = "От 1000")]
		From1000 = 5,

		[FilterByPrice(0, false, true)]
		[Display(Name = "Со скидкой")]
		Discounted = 6,
	}
}
