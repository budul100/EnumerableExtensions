using HashExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

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

            return result ?? Enumerable.Empty<IEnumerable<T>>();
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

        public static IEnumerable<IEnumerable<T>> SplitAtChange<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> property)
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

        #endregion Public Methods
    }
}