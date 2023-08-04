using System.Collections.Generic;

namespace LiteralLifeChurch.ArchiveManagerApi.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
    {
        foreach (var item in items)
            if (item is object)
                yield return item;
    }
}
