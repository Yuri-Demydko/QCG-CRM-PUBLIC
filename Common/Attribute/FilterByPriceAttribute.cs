namespace Common.Attribute
{
	public class FilterByPriceAttribute : System.Attribute
    {
        public int Price { get; }
        public bool IsFrom { get; }
        public bool HasDiscount { get; }

		public FilterByPriceAttribute(int price, bool isFrom, bool hasDiscount = false)
		{
			Price = price;
			IsFrom = isFrom;
			HasDiscount = hasDiscount;
		}
	}
}
