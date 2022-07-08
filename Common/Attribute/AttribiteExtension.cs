using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Attribute
{
	public static class AttribiteExtension
	{
        public static FilterByPriceAttribute GetFilterByPriceInfo (this FilterByPrice status)
        {
            Type filterByPriceType = typeof(FilterByPrice);
            string enumName = System.Enum.GetName(filterByPriceType, status);
            MemberInfo[] memberInfo = filterByPriceType.GetMember(enumName);

            if (memberInfo.Length != 1)
            {
                throw new ArgumentException($"Attrubite of {status} should only have one memberInfo");
            }

            IEnumerable<FilterByPriceAttribute> customAttributes = memberInfo[0].GetCustomAttributes<FilterByPriceAttribute>();
            FilterByPriceAttribute filterByPrice = customAttributes.FirstOrDefault();

            if (filterByPrice == null)
            {
                throw new InvalidOperationException($"Filter of {status} has no attribute");
            }

            return filterByPrice;
        }
    }
}
