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
            var result = new T[] { value };

            return result;
        }

        public static T[] AsArrayOrDefault<T>(this IEnumerable<T> items)
        {
            var result = items?.ToArray();

            if (!result.AnyItem())
            {
                result = default;
            }

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

        public static IEnumerable<T> AsEnumerable<T>(this T value)
        {
            var result = new T[] { value };

            return result;
        }

        public static List<T> AsList<T>(this T value)
        {
            var result = new List<T> { value };

            return result;
        }

        public static List<T> AsListOrDefault<T>(this IEnumerable<T> items)
        {
            var result = items?.ToList();

            if (!result.AnyItem())
            {
                result = default;
            }

            return result;
        }

        #endregion Public Methods
    }
}