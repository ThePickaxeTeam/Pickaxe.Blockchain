using System.Text;

namespace Pickaxe.Blockchain.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
