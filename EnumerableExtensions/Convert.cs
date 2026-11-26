using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static T[] AsArray<T>(this T value)
        {
            return [value];
        }

        public static List<T> AsList<T>(this T value)
        {
            return [value];
        }

        /// <summary>
        /// Wraps a single value into an enumerable.
        /// Renamed from AsEnumerable, which hijacked System.Linq.Enumerable.AsEnumerable
        /// for arrays and lists (identity conversion beats reference conversion).
        /// </summary>
        public static IEnumerable<T> AsSingleEnumerable<T>(this T value)
        {
            return [value];
        }

        public static T[] ToArrayOrDefault<T>(this IEnumerable<T> items)
        {
            var result = items?.ToArray();

            return result is { Length: > 0 }
                ? result
                : default;
        }

        /// <summary>
        /// Keys must not be null, the underlying dictionary rejects them.
        /// </summary>
        public static IDictionary<TKey, IEnumerable<T>> ToGroupedDictionary<T, TKey>(this IEnumerable<T> items,
            Func<T, TKey> keyGetter)
        {
            ArgumentNullException.ThrowIfNull(keyGetter);

            var result = new Dictionary<TKey, IEnumerable<T>>();

            foreach (var keyGroup in items.IfAny().GroupBy(keyGetter))
            {
                result.Add(
                    key: keyGroup.Key,
                    value: keyGroup);
            }

            return result;
        }

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> items)
        {
            var result = items?.ToList();

            return result is { Count: > 0 }
                ? result
                : default;
        }

        #endregion Public Methods
    }
}
