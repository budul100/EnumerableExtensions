using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static int Count(this IEnumerable items)
    {
        if (items is null)
        {
            return 0;
        }

        if (items is ICollection collection)
        {
            return collection.Count;
        }

        return Count(items.GetEnumerator());
    }

    /// <summary>
    /// True if all sequences with more than one element have the same length.
    /// Null, empty and single-element sequences are ignored.
    /// </summary>
    public static bool CountEqualsOrSingle(this IEnumerable items,
        params IEnumerable[] others)
    {
        var all = 0;

        foreach (var current in GetCounts(items, others))
        {
            if (current > 1)
            {
                if (all == 0)
                {
                    all = current;
                }
                else if (current != all)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static T ElementAtOrSingle<T>(this IEnumerable<T> items, int index)
    {
        if (items is null)
        {
            return default;
        }

        // At most one materialization, none for lists and arrays.
        var list = items as IList<T> ?? items.ToArray();

        if (list.Count == 0)
        {
            return default;
        }

        return list.Count == 1
            ? list[0]
            : list[index];
    }

    public static IEnumerable<int> Indexes(this IEnumerable items, params IEnumerable[] others)
    {
        var length = 0;

        foreach (var current in GetCounts(items, others))
        {
            if (current > length)
            {
                length = current;
            }
        }

        for (var index = 0; index < length; index++)
        {
            yield return index;
        }
    }

    #endregion Public Methods

    #region Private Methods

    private static int Count(IEnumerator enumerator)
    {
        var result = 0;

        // Enumerators of iterators are disposable, but the non-generic interface does not say so.
        using (enumerator as IDisposable)
        {
            while (enumerator.MoveNext())
            {
                result++;
            }
        }

        return result;
    }

    private static IEnumerable<int> GetCounts(IEnumerable items, IEnumerable[] others)
    {
        if (items is not null)
        {
            yield return Count(items);
        }

        foreach (var other in others.IfAny())
        {
            if (other is not null)
            {
                yield return Count(other);
            }
        }
    }

    #endregion Private Methods
}
