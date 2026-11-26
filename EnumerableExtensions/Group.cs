using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static IEnumerable<IEnumerable<T>> ChunkAfter<T>(this IEnumerable<T> items,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return GetChunksAfter(
            items: items,
            predicate: predicate);
    }

    public static IEnumerable<IEnumerable<T>> ChunkBefore<T>(this IEnumerable<T> items,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return GetChunksBefore(
            items: items,
            predicate: predicate);
    }

    public static IEnumerable<IEnumerable<T>> Framed<T>(this IEnumerable<T> items,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return items
            .SplitAt(predicate)
            .Where(g => predicate(g.First())
                && predicate(g.Last()));
    }

    /// <summary>
    /// Groups by the values of all given properties.
    /// Uses structural equality of the value tuple, so (unlike a plain hash key)
    /// hash collisions can never merge unequal items.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> GroupByHash<T, TProperty>(this IEnumerable<T> items,
        params Func<T, TProperty>[] properties)
    {
        ArgumentNullException.ThrowIfNull(properties);

        if (items is null)
        {
            return [];
        }

        var result = items
            .GroupBy(
                keySelector: s => GetKey(s, properties),
                comparer: new SequenceKeyComparer<TProperty>())
            .ToArray();

        return result;
    }

    public static IEnumerable<IEnumerable<T>> SplitAt<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return GetSplitsAt(
            items: items,
            predicate: predicate);
    }

    public static IEnumerable<IEnumerable<T>> SplitAtChange<T, TProperty>(this IEnumerable<T> items,
        Func<T, TProperty> property)
    {
        ArgumentNullException.ThrowIfNull(property);

        return GetSplitsAtChange(
            items: items,
            property: property);
    }

    #endregion Public Methods

    #region Private Methods

    private static IEnumerable<IEnumerable<T>> GetChunksAfter<T>(IEnumerable<T> items, Func<T, bool> predicate)
    {
        var result = new List<T>();

        foreach (var item in items.IfAny())
        {
            result.Add(item);

            if (predicate(item))
            {
                yield return result;
                result = [];
            }
        }

        if (result.Count > 0)
        {
            yield return result;
        }
    }

    private static IEnumerable<IEnumerable<T>> GetChunksBefore<T>(IEnumerable<T> items, Func<T, bool> predicate)
    {
        var result = new List<T>();

        foreach (var item in items.IfAny())
        {
            if (result.Count > 0
                && predicate(item))
            {
                yield return result;
                result = [];
            }

            result.Add(item);
        }

        if (result.Count > 0)
        {
            yield return result;
        }
    }

    private static TProperty[] GetKey<T, TProperty>(T item, Func<T, TProperty>[] properties)
    {
        var result = new TProperty[properties.Length];

        for (var index = 0; index < properties.Length; index++)
        {
            result[index] = properties[index](item);
        }

        return result;
    }

    private static IEnumerable<IEnumerable<T>> GetSplitsAt<T>(IEnumerable<T> items, Func<T, bool> predicate)
    {
        var result = new List<T>();
        var anyFound = false;

        foreach (var item in items.IfAny())
        {
            result.Add(item);

            if (result.Count > 1
                && predicate(item))
            {
                yield return result;

                result = [item];
                anyFound = true;
            }
        }

        if (result.Count > 1
            || (!anyFound && result.Count > 0))
        {
            yield return result;
        }
    }

    private static IEnumerable<IEnumerable<T>> GetSplitsAtChange<T, TProperty>(IEnumerable<T> items,
        Func<T, TProperty> property)
    {
        var result = new List<T>();
        var last = default(TProperty);

        foreach (var item in items.IfAny())
        {
            var current = property(item);

            if (result.Count > 0
                && !current.IsEqual(last))
            {
                yield return result;
                result = [];
            }

            result.Add(item);
            last = current;
        }

        if (result.Count > 0)
        {
            yield return result;
        }
    }

    #endregion Private Methods
}
