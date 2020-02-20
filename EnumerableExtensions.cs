﻿using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class EnumerableExtensions
    {
        #region Public Methods

        public static bool AnyItem<T>(this IEnumerable<T> items)
        {
            var result = items?.Any() ?? false;

            return result;
        }

        public static bool AnyNonDefaultItem<T>(this IEnumerable<T> items)
        {
            var result = items?.Any(s => !s.IsDefault()) ?? false;

            return result;
        }

        public static IDictionary<U, IEnumerable<T>> AsDictionary<T, U>(this IEnumerable<T> source, Func<T, U> keyGetter)
        {
            var result = new Dictionary<U, IEnumerable<T>>();

            var keyGroups = source
                .GroupBy(s => keyGetter.Invoke(s)).ToArray();

            foreach (var keyGroup in keyGroups)
            {
                result.Add(
                    key: keyGroup.Key,
                    value: keyGroup);
            }

            return result;
        }

        public static IEnumerable<IList<T>> ChunkAfter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                result.Add(item);

                if (result.Any() && (predicate?.Invoke(item) ?? false))
                {
                    yield return result;
                    result = new List<T>();
                }
            }

            if (result.Any())
            {
                yield return result;
            }
        }

        public static IEnumerable<IList<T>> ChunkBefore<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                if (result.Any() && (predicate?.Invoke(item) ?? false))
                {
                    yield return result;
                    result = new List<T>();
                }

                result.Add(item);
            }

            if (result.Any())
            {
                yield return result;
            }
        }

        public static IEnumerable<U> Consecutive<T, U>(this IEnumerable<T> source, Func<T, T, U> getter)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (getter == null)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (source.Any())
            {
                var previous = default(T);

                using (var e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (!previous.IsDefault())
                            yield return getter(
                                arg1: previous,
                                arg2: e.Current);

                        previous = e.Current;
                    }

                    yield return getter(
                        arg1: previous,
                        arg2: default);
                }
            }
        }

        public static void CreateUnique<T>(this IEnumerable<T> items, Func<T, string> getter, Action<T, string> setter)
        {
            var basis = items
                .Select(c => getter?.Invoke(c))
                .Distinct().Single();

            var digits = (int)Math.Floor(Math.Log10(items.Count() + 1) + 1);

            var format = $"D{digits}";

            var index = 1;
            foreach (var item in items)
            {
                var value = basis + index.ToString(format);
                setter?.Invoke(
                    arg1: item,
                    arg2: value);

                index++;
            }
        }

        public static T ElementAtOrFirst<T>(this IEnumerable<T> items, int index)
        {
            var result = default(T);

            if (items?.Any() ?? false)
            {
                result = items.Count() == 1
                    ? items.ElementAt(0)
                    : items.ElementAt(index);
            }

            return result;
        }

        public static bool EqualsOrDefault<T>(this IEnumerable<T> x, IEnumerable<T> y)
        {
            return x == default
                || (x?.GetSequenceHashOrdered() ?? 0) == (y?.GetSequenceHashOrdered() ?? 0);
        }

        public static TProp FirstNonNullOrDefault<T, TProp>(this IEnumerable<T> items, Func<T, TProp> property)
        {
            return items
                .Select(property)
                .FirstOrDefault(s => !s.IsDefault());
        }

        public static IEnumerable<IEnumerable<T>> Framed<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items?.Any() ?? false)
            {
                var result = new List<T>();

                foreach (var item in items)
                {
                    if (result.Any()
                        || (predicate?.Invoke(item) ?? false))
                    {
                        result.Add(item);
                    }

                    if (result.Count() > 1
                        && (predicate?.Invoke(item) ?? false))
                    {
                        yield return result;

                        result = new List<T>
                    {
                        item
                    };
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupByHash<T, TProperty>(this IEnumerable<T> source, params Func<T, TProperty>[] properties)
        {
            return source.ToArray()
                .GroupBy(s => properties.Select(p => p(s)).GetSequenceHash()).ToArray();
        }

        public static IEnumerable<T> IfAny<T>(this IEnumerable<T> sequence)
        {
            if (sequence?.Any() ?? false)
            {
                foreach (var s in sequence)
                {
                    yield return s;
                }
            }
        }

        public static IEnumerable<TResult> Paired<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TSource, TResult> pairs)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pairs == null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            if (source.Any())
            {
                using (var e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        yield break;
                    }

                    var previous = e.Current;
                    while (e.MoveNext())
                    {
                        yield return pairs(
                            arg1: previous,
                            arg2: e.Current);

                        previous = e.Current;
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Segmented<T>(this IEnumerable<IEnumerable<T>> groups)
        {
            var given = groups
                .SelectMany(g => g)
                .Where(g => !g.IsDefault()).ToArray();

            while (given?.Count() > 0)
            {
                given = given
                    .OrderBy(b => b)
                    .ToArray();

                var result = groups.GetItemGroup(
                    items: given,
                    item: given.First())
                    .Distinct().ToArray();

                yield return result;

                given = given
                    .Except(result)
                    .ToArray();
            }
        }

        public static bool SequenceEqualNullable<T>(this IEnumerable<T> current, IEnumerable<T> other,
            IEqualityComparer<T> comparer = null)
        {
            if (current.AnyItem() && other.AnyItem())
            {
                return comparer == null
                    ? current.SequenceEqual(other)
                    : current.SequenceEqual(
                        second: other,
                        comparer: comparer);
            }
            else
            {
                return !current.AnyItem() && !other.AnyItem();
            }
        }

        public static IEnumerable<IList<T>> SplitAtChange<T, TProperty>(this IEnumerable<T> source,
            Func<T, TProperty> property)
        {
            var result = new List<T>();
            var last = default(TProperty);

            foreach (var item in source)
            {
                var current = property != null
                    ? property(item)
                    : default;

                if (result.Any()
                    && current.IsEqual(last))
                {
                    yield return result;
                    result = new List<T>();
                }

                result.Add(item);
                last = current;
            }

            if (result.Any())
            {
                yield return result;
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            yield return value;
        }

        #endregion Public Methods

        #region Private Methods

        private static IEnumerable<T> GetItemGroup<T>(this IEnumerable<IEnumerable<T>> groups, IEnumerable<T> items,
            T item)
        {
            var givenGroups = groups
                .Where(g => g.Contains(item))
                .ToArray();

            foreach (var b in items)
            {
                var newGroups = groups
                    .Where(g => g.Contains(b))
                    .ToArray();

                if (newGroups.Count() == givenGroups.Count())
                {
                    var i = 0;
                    while (i < newGroups.Count())
                    {
                        if (!newGroups[i].SequenceEqual(givenGroups[i]))
                        {
                            break;
                        }

                        i++;
                    }

                    if (i == newGroups.Count())
                    {
                        yield return b;
                    }
                }
            }
        }

        private static bool IsDefault<T>(this T x)
        {
            return x.IsEqual(default);
        }

        private static bool IsEqual<T>(this T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(
                x: x,
                y: y);
        }

        #endregion Private Methods
    }
}