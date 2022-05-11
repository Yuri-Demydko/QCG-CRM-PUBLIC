using System.Numerics;
using ExtendedNumerics;

namespace Sia.Helpers
{
    public class HastingsHelper
    {
        private const int SiaToHastingsPow = 24;
        public static decimal HastingsToCoins(string value)
        {
            var bigIntValue = BigInteger.Parse(value);
           

            return (decimal) BigRational.Divide(bigIntValue, BigInteger.Pow(10, SiaToHastingsPow));
        }
    }
}