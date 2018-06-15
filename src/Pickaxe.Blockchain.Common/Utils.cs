using System.Linq;
using System.Text;

namespace Pickaxe.Blockchain.Common
{
    public static class Utils
    {
        public static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ToHex(byte[] value)
        {
            return string.Concat(value.Select(b => b.ToString("X2")));
        }

        public static string ToUTF8String(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
