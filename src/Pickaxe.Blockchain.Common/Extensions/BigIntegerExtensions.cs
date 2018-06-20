using Org.BouncyCastle.Math;

namespace Pickaxe.Blockchain.Common.Extensions
{
    public static class BigIntegerExtensions
    {
        public static bool IsOdd(this BigInteger value)
        {
            BigInteger remainder = value.Remainder(BigInteger.Two);
            return remainder.CompareTo(BigInteger.One) == 0;
        }
    }
}
