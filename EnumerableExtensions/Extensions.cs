using HashExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class Extensions
    {
        #region Public Methods

        public static bool AnyItem<T>(this IEnumerable<T> items)
        {
            var result = items?.Any() ?? false;

            return result;
        }

        public static bool AnyNonDefaultItem<T>(this IEnumerable<T> items)
        {
            var result = items?.Any(s => !s.IsDefault())
                ?? false;

            return result;
        }

        public static IDictionary<TKey, IEnumerable<T>> AsDictionary<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyGetter)
        {
            if (keyGetter == default)
            {
                throw new ArgumentNullException(nameof(keyGetter));
            }

            var result = new Dictionary<TKey, IEnumerable<T>>();

            var keyGroups = items?
                .GroupBy(s => keyGetter.Invoke(s)).ToArray();

            foreach (var keyGroup in keyGroups.IfAny())
            {
                result.Add(
                    key: keyGroup.Key,
                    value: keyGroup);
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> ChunkAfter<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<T>();

            foreach (var item in items.IfAny())
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

        public static IEnumerable<IEnumerable<T>> ChunkBefore<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<T>();

            foreach (var item in items.IfAny())
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

        public static IEnumerable<TNew> Consecutive<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
        {
            if (getter == default)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (items.AnyItem())
            {
                var previous = default(T);

                using (var enumerator = items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (!previous.IsDefault())
                        {
                            yield return getter.Invoke(
                                arg1: previous,
                                arg2: enumerator.Current);
                        }

                        previous = enumerator.Current;
                    }

                    yield return getter.Invoke(
                        arg1: previous,
                        arg2: default);
                }
            }
        }

        public static IEnumerable<T> DistinctSuccessive<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = default)
        {
            if (items.AnyItem())
            {
                bool equals(T left, T right) => comparer == default
                  ? left.Equals(right)
                  : comparer.Equals(left, right);

                var first = true;
                var prior = default(T);

                foreach (var item in items)
                {
                    var isDifferent = first || !equals(
                        left: item,
                        right: prior);

                    if (isDifferent)
                        yield return item;

                    first = false;
                    prior = item;
                }
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

        public static bool EqualsOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> other)
        {
            var result = true;

            if (current.AnyItem())
            {
                var currentSet = new HashSet<T>(current);
                result = currentSet.SetEquals(other);
            }

            return result;
        }

        public static TProperty FirstNonNullOrDefault<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> property)
        {
            return items
                .Select(property)
                .FirstOrDefault(s => !s.IsDefault());
        }

        public static IEnumerable<IEnumerable<T>> Framed<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<T>();

            foreach (var item in items.IfAny())
            {
                if (result.Any()
                    || (predicate?.Invoke(item) ?? false))
                {
                    result.Add(item);
                }

                if (result.Count > 1
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

        public static IEnumerable<IEnumerable<T>> GroupByHash<T, TProperty>(this IEnumerable<T> items,
            params Func<T, TProperty>[] properties)
        {
            if (properties == default)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var result = items?
                .GroupBy(s => properties.GetSequenceHash(p => p(s))).ToArray();

            return result
                ?? Enumerable.Empty<IEnumerable<T>>();
        }

        public static IEnumerable<T> IfAny<T>(this IEnumerable<T> items)
        {
            if (items.AnyItem())
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }

        public static string Merge(string delimiter, params string[] items)
        {
            return items.Merge(delimiter);
        }

        public static string Merge<T, TProp>(this IEnumerable<T> items, Func<T, TProp> property, string delimiter = ",")
        {
            var result = default(string);

            if (items.AnyItem())
            {
                result = items
                    .Select(i => property?.Invoke(i)?.ToString())
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Merge(delimiter);
            }

            return result;
        }

        public static string Merge<T>(this IEnumerable<T> items, string delimiter = ",")
        {
            var result = string.Join(
                separator: delimiter,
                values: items);

            return result;
        }

        public static IEnumerable<TNew> Paired<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
        {
            if (getter == default)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (items.AnyItem())
            {
                using (var enumerator = items.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }

                    var previous = enumerator.Current;
                    while (enumerator.MoveNext())
                    {
                        yield return getter(
                            arg1: previous,
                            arg2: enumerator.Current);

                        previous = enumerator.Current;
                    }
                }
            }
        }

        public static bool SequenceEqualNullable<T>(this IEnumerable<T> current, IEnumerable<T> other,
            IEqualityComparer<T> comparer = default)
        {
            if (current.AnyItem() && other.AnyItem())
            {
                return comparer == default
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

        public static IEnumerable<IEnumerable<T>> SplitAt<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<T>();

            foreach (var item in items.IfAny())
            {
                result.Add(item);

                if (result.Count > 1
                    && (predicate?.Invoke(item) ?? false))
                {
                    yield return result;

                    result = new List<T>
                        {
                            item
                        };
                }
            }

            if (result.Count > 1)
            {
                yield return result;
            }
        }

        public static IEnumerable<IList<T>> SplitAtChange<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> property)
        {
            if (property == default)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var result = new List<T>();

            var last = default(TProperty);

            foreach (var item in items.IfAny())
            {
                var current = property != default
                    ? property(item)
                    : default;

                if (result.Any()
                    && !current.IsEqual(last))
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

        public static T[] ToArrayOrDefault<T>(this IEnumerable<T> values)
        {
            var relevants = values?.ToArray();

            var result = relevants.AnyItem()
                ? relevants
                : default;

            return result;
        }

        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            var result = new T[] { value };

            return result;
        }

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> values)
        {
            var relevants = values?.ToList();

            var result = relevants.AnyItem()
                ? relevants
                : default;

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static bool IsDefault<T>(this T item)
        {
            return item.IsEqual(default);
        }

        private static bool IsEqual<T>(this T current, T other)
        {
            return EqualityComparer<T>.Default.Equals(
                x: current,
                y: other);
        }

        #endregion Private Methods
    }
}