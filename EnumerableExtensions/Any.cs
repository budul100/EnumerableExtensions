using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static bool AnyItem<T>(this IEnumerable<T> items)
    {
        return items?.Any() == true;
    }

    public static bool AnyItemNonDefault<T>(this IEnumerable<T> items)
    {
        return items?.Any(i => !i.IsDefault()) == true;
    }

    public static IEnumerable<T> IfAny<T>(this IEnumerable<T> items)
    {
        // Returning the source itself keeps the ICollection fast paths (Count, ToArray) intact.
        return items ?? [];
    }

    public static IEnumerable<T> IfAnyNonDefault<T>(this IEnumerable<T> items)
    {
        if (items is null)
        {
            return [];
        }

        // Buffered on purpose: the source is enumerated exactly once.
        var buffer = items.ToArray();

        return buffer.AnyItemNonDefault()
            ? buffer
            : Enumerable.Empty<T>();
    }

    public static IEnumerable<T> NonDefaults<T>(this IEnumerable<T> items)
    {
        return items is null
            ? []
            : items.Where(static i => !i.IsDefault());
    }

    #endregion Public Methods
}
