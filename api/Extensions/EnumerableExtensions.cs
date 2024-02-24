using System;
using System.Collections.Generic;

namespace LiteralLifeChurch.ArchiveManagerApi.Extensions;

public static class EnumerableExtensions
{
    public static void ForEachIndexed<T>(this IEnumerable<T> sequence, Action<int, T> action)
    {
        var i = 0;

        foreach (var item in sequence)
        {
            action(i, item);
            i++;
        }
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
    {
        foreach (var item in items)
            if (item is object)
                yield return item;
    }
}
