using System.Collections.Generic;
using System.Linq;

namespace Pickaxe.Blockchain.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }
    }
}
