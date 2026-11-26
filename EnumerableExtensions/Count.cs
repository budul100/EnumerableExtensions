using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static int CountOrDefault<T>(this IEnumerable<T> items)
        {
            return items?.Count() ?? 0;
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> items)
        {
            // DefaultIfEmpty keeps this at a single enumeration.
            return items is null
                ? default
                : items.DefaultIfEmpty().Max();
        }

        public static T? MaxOrNull<T>(this IEnumerable<T> items)
            where T : struct
        {
            return items?
                .Select(static i => (T?)i)
                .Max();
        }

        public static T MinOrDefault<T>(this IEnumerable<T> items)
        {
            return items is null
                ? default
                : items.DefaultIfEmpty().Min();
        }

        public static T? MinOrNull<T>(this IEnumerable<T> items)
            where T : struct
        {
            return items?
                .Select(static i => (T?)i)
                .Min();
        }

        #endregion Public Methods
    }
}
