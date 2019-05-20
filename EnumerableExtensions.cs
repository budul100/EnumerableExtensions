using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        #region Public Methods

        public static bool AnyItem<T>
            (this IEnumerable<T> items)
        {
            var result = items?.Any() ?? false;

            return result;
        }

        public static bool AnyNonDefault<T>
            (this IEnumerable<T> items)
        {
            var result = items?.Any(s => !s.IsDefault()) ?? false;

            return result;
        }

        public static IEnumerable<IList<T>> ChunkAfter<T>
            (this IEnumerable<T> source, Func<T, bool> property)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                result.Add(item);

                if (property?.Invoke(item) ?? false && result.Any())
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

        public static IEnumerable<IList<T>> ChunkBefore<T>
            (this IEnumerable<T> source, Func<T, bool> property)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                if (property?.Invoke(item) ?? false && result.Any())
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

        public static IEnumerable<U> Consecutive<T, U>
            (this IEnumerable<T> source, Func<T, T, U> getter)
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

        public static bool EqualsOrDefault<T>
            (this IEnumerable<T> x, IEnumerable<T> y)
        {
            return x == default
                || (x?.GetSequenceHashOrdered() ?? 0) == (y?.GetSequenceHashOrdered() ?? 0);
        }

        public static TProp FirstNonNullOrDefault<T, TProp>
            (this IEnumerable<T> items, Func<T, TProp> property)
        {
            return items
                .Select(property)
                .FirstOrDefault(s => !s.IsDefault());
        }

        public static T GetElementAt<T>
            (this IEnumerable<T> items, int index)
        {
            return items?.Any() ?? false
                ? items.ElementAt(index)
                : default;
        }

        public static IEnumerable<T> GetIfAny<T>
            (this IEnumerable<T> sequence)
        {
            if (sequence?.Any() ?? false)
            {
                foreach (var s in sequence)
                {
                    yield return s;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupByHash<T, TProperty>
            (this IEnumerable<T> source, params Func<T, TProperty>[] properties)
            where TProperty : class
        {
            return source
                .GroupBy(s => properties.Select(p => p(s)).GetSequenceHash())
                .ToArray();
        }

        public static IEnumerable<TResult> Pairwise<TSource, TResult>
            (this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
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
                        yield return resultSelector(previous, e.Current);
                        previous = e.Current;
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Segmented<T>
            (this IEnumerable<IEnumerable<T>> groups)
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

        public static bool SequenceEqualNullable<T>
            (this IEnumerable<T> current, IEnumerable<T> other,
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

        public static IEnumerable<IEnumerable<T>> SplitAt<T>
            (this IEnumerable<T> items, Func<T, bool> predicate)
        {
            var from = 0;

            do
            {
                var current = items.GetSplittedTo(
                    predicate: predicate,
                    from: from).ToList();
                if (!current.Any())
                {
                    yield break;
                }

                yield return current;

                from = from + current.Count() - 1;
            } while (true);
        }

        public static IEnumerable<IEnumerable<T>> SplitAt<T>
            (this IEnumerable<IEnumerable<T>> items, Func<T, bool> predicate)
        {
            foreach (var s in items)
            {
                var current = s
                    .SplitAt(predicate)
                    .ToList();

                foreach (var c in current)
                {
                    yield return c;
                }
            }
        }

        public static IEnumerable<IList<T>> SplitAtChange<T, TProperty>
            (this IEnumerable<T> source, Func<T, TProperty> property)
        {
            var result = new List<T>();
            var last = default(TProperty);

            foreach (var item in source)
            {
                var current = property != null
                    ? property(item)
                    : default;

                if (result.Count > 0
                    && current.IsEqual(last))
                {
                    yield return result;
                    result = new List<T>();
                }

                result.Add(item);
                last = current;
            }

            if (result.Count > 0)
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

        private static IEnumerable<T> GetItemGroup<T>
            (this IEnumerable<IEnumerable<T>> groups, IEnumerable<T> items, T item)
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

        private static IEnumerable<T> GetSplittedTo<T>
            (this IEnumerable<T> items, Func<T, bool> predicate, int from)
        {
            if (from < items.Count() - 1)
            {
                for (int i = from; i < items.Count(); i++)
                {
                    var current = items.ElementAt(i);

                    yield return current;

                    if (i > from && (predicate?.Invoke(current) ?? false))
                    {
                        yield break;
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