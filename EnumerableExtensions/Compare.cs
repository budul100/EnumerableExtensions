using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static bool ContainsOrEmpty<T>(this IEnumerable<T> items, T item)
    {
        if (items is null)
        {
            return true;
        }

        // Keeps O(1) lookups for sets.
        if (items is ICollection<T> collection)
        {
            return collection.Count == 0
                || collection.Contains(item);
        }

        var any = false;

        foreach (var current in items)
        {
            any = true;

            if (current.IsEqual(item))
            {
                return true;
            }
        }

        return !any;
    }

    /// <summary>
    /// Returns true if current is null. Otherwise both sides are compared as sets,
    /// a null other side never equals a non-null current side.
    /// </summary>
    public static bool EqualsOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> other)
    {
        if (current is null)
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        var currentSet = new HashSet<T>(current);

        return currentSet.SetEquals(other);
    }

    /// <summary>
    /// Returns the first property value which is not the default of its type
    /// (so 0 or false are skipped as well).
    /// </summary>
    public static TProperty FirstNonNullOrDefault<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> property)
    {
        ArgumentNullException.ThrowIfNull(property);

        foreach (var item in items.IfAny())
        {
            var value = property(item);

            if (!value.IsDefault())
            {
                return value;
            }
        }

        return default;
    }

    public static bool SequenceEqualOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> other,
        IEqualityComparer<T> comparer = default)
    {
        if (current is not null
            && other is not null)
        {
            return current.SequenceEqual(
                second: other,
                comparer: comparer);
        }
        else
        {
            return current is null
                && other is null;
        }
    }

    #endregion Public Methods
}
