using System;

namespace Pickaxe.Blockchain.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string Iso8601Formatted(this DateTime utcDate)
        {
            return utcDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
    }
}
