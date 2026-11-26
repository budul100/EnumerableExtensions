using System;
using System.Collections.Generic;

namespace EnumerableExtensions;

public static partial class Extensions
{
    #region Public Methods

    public static IEnumerable<T> DistinctSuccessive<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = default)
    {
        if (items is null)
        {
            return [];
        }

        return GetDistinctSuccessive(
            items: items,
            comparer: comparer ?? EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Emits n + 1 pairs: (default, first), (first, second) ... (last, default).
    /// Returns nothing for an empty sequence.
    /// </summary>
    public static IEnumerable<TNew> ToConsecutivePairs<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
    {
        ArgumentNullException.ThrowIfNull(getter);

        return items is null
            ? []
            : GetConsecutivePairs(
                items: items,
                getter: getter);
    }

    /// <summary>
    /// Emits n + 2 triples, padded with default at the start and at the end.
    /// Returns nothing for an empty sequence.
    /// </summary>
    public static IEnumerable<TNew> ToConsecutiveTriples<T, TNew>(this IEnumerable<T> items, Func<T, T, T, TNew> getter)
    {
        ArgumentNullException.ThrowIfNull(getter);

        return items is null
            ? []
            : GetConsecutiveTriples(
                items: items,
                getter: getter);
    }

    public static IEnumerable<TNew> ToPairs<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
    {
        ArgumentNullException.ThrowIfNull(getter);

        return items is null
            ? []
            : GetPairs(
                items: items,
                getter: getter);
    }

    public static IEnumerable<TNew> ToTriples<T, TNew>(this IEnumerable<T> items, Func<T, T, T, TNew> getter)
    {
        ArgumentNullException.ThrowIfNull(getter);

        return items is null
            ? []
            : GetTriples(
                items: items,
                getter: getter);
    }

    #endregion Public Methods

    #region Private Methods

    private static bool AreEqual<T>(IEqualityComparer<T> comparer, T left, T right)
    {
        var leftIsNull = left is null;
        var rightIsNull = right is null;

        // Custom comparers are never confronted with null values.
        return leftIsNull || rightIsNull
            ? leftIsNull && rightIsNull
            : comparer.Equals(
                x: left,
                y: right);
    }

    private static IEnumerable<TNew> GetConsecutivePairs<T, TNew>(IEnumerable<T> items, Func<T, T, TNew> getter)
    {
        var previous = default(T);
        var any = false;

        foreach (var item in items)
        {
            any = true;

            yield return getter(
                arg1: previous,
                arg2: item);

            previous = item;
        }

        if (any)
        {
            yield return getter(
                arg1: previous,
                arg2: default);
        }
    }

    private static IEnumerable<TNew> GetConsecutiveTriples<T, TNew>(IEnumerable<T> items, Func<T, T, T, TNew> getter)
    {
        var previous1 = default(T);
        var previous2 = default(T);
        var any = false;

        foreach (var item in items)
        {
            any = true;

            yield return getter(
                arg1: previous2,
                arg2: previous1,
                arg3: item);

            previous2 = previous1;
            previous1 = item;
        }

        if (any)
        {
            yield return getter(
                arg1: previous2,
                arg2: previous1,
                arg3: default);

            yield return getter(
                arg1: previous1,
                arg2: default,
                arg3: default);
        }
    }

    private static IEnumerable<T> GetDistinctSuccessive<T>(IEnumerable<T> items, IEqualityComparer<T> comparer)
    {
        var first = true;
        var prior = default(T);

        foreach (var item in items)
        {
            if (first || !AreEqual(comparer, item, prior))
            {
                yield return item;
            }

            first = false;
            prior = item;
        }
    }

    private static IEnumerable<TNew> GetPairs<T, TNew>(IEnumerable<T> items, Func<T, T, TNew> getter)
    {
        var previous = default(T);
        var first = true;

        foreach (var item in items)
        {
            if (!first)
            {
                yield return getter(
                    arg1: previous,
                    arg2: item);
            }

            previous = item;
            first = false;
        }
    }

    private static IEnumerable<TNew> GetTriples<T, TNew>(IEnumerable<T> items, Func<T, T, T, TNew> getter)
    {
        var previous1 = default(T);
        var previous2 = default(T);
        var buffered = 0;

        foreach (var item in items)
        {
            if (buffered >= 2)
            {
                yield return getter(
                    arg1: previous2,
                    arg2: previous1,
                    arg3: item);
            }
            else
            {
                buffered++;
            }

            previous2 = previous1;
            previous1 = item;
        }
    }

    #endregion Private Methods
}
