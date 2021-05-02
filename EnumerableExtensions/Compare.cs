using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static bool ContainsOrEmpty<T>(this IEnumerable<T> items, T item)
        {
            var result = true;

            if (items?.Any() ?? false)
            {
                result = items.Contains(item);
            }

            return result;
        }

        public static bool EqualsOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> other)
        {
            var result = true;

            if (current != default)
            {
                var currentSet = new HashSet<T>(current);
                result = currentSet.SetEquals(other);
            }

            return result;
        }

        public static TProperty FirstNonNullOrDefault<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> property)
        {
            var result = items
                .Select(property)
                .FirstOrDefault(s => !s.IsDefault());

            return result;
        }

        public static bool SequenceEqualOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> other,
            IEqualityComparer<T> comparer = default)
        {
            if (current != default
                && other != default)
            {
                return comparer == default
                    ? current.SequenceEqual(other)
                    : current.SequenceEqual(
                        second: other,
                        comparer: comparer);
            }
            else
            {
                return current == default
                    && other == default;
            }
        }

        #endregion Public Methods
    }
}