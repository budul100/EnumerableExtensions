using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static string Merge(this string delimiter, params string[] items)
    {
        var result = items.Merge(
            delimiter: delimiter,
            preventDistinct: true,
            preventSort: true);

        return result;
    }

    public static string Merge<T, TProp>(this IEnumerable<T> items, Func<T, TProp> property, string delimiter = ",",
        bool preventDistinct = false, bool preventSort = false)
    {
        ArgumentNullException.ThrowIfNull(property);

        if (items is null)
        {
            return default;
        }

        var result = items
            .Where(static i => i is not null)
            .Select(i => property(i))
            .Merge(
                delimiter: delimiter,
                preventDistinct: preventDistinct,
                preventSort: preventSort);

        return result;
    }

    /// <summary>
    /// Joins the string representations of all non-null items. Whitespace-only texts are skipped,
    /// the result is null if nothing is left. Sorting uses the natural order of T where available
    /// (ordinal for strings), otherwise the ordinal order of the texts.
    /// </summary>
    public static string Merge<T>(this IEnumerable<T> items, string delimiter = ",", bool preventDistinct = false,
        bool preventSort = false)
    {
        if (items is null)
        {
            return default;
        }

        var relevants = items
            .Where(static i => i is not null);

        var comparer = preventSort
            ? default
            : GetSortComparer<T>();

        if (comparer is not null)
        {
            relevants = relevants
                .OrderBy(
                    keySelector: static i => i,
                    comparer: comparer);
        }

        var texts = relevants
            .Select(static i => i.ToString())
            .Where(static t => !string.IsNullOrWhiteSpace(t));

        if (!preventSort
            && comparer is null)
        {
            texts = texts
                .OrderBy(
                    keySelector: static t => t,
                    comparer: StringComparer.Ordinal);
        }

        if (!preventDistinct)
        {
            texts = texts.Distinct();
        }

        var result = texts.ToArray();

        return result.Length > 0
            ? string.Join(
                separator: delimiter,
                value: result)
            : default;
    }

    #endregion Public Methods

    #region Private Methods

    private static IComparer<T> GetSortComparer<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return (IComparer<T>)(object)StringComparer.Ordinal;
        }

        return typeof(IComparable<T>).IsAssignableFrom(typeof(T))
            || typeof(System.IComparable).IsAssignableFrom(typeof(T))
            ? Comparer<T>.Default
            : default;
    }

    #endregion Private Methods
}
