using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static bool AnyItem<T>(this IEnumerable<T> items)
        {
            var result = false;

            if (items != default)
            {
                result = items.Any();
            }

            return result;
        }

        public static bool AnyItemNonDefault<T>(this IEnumerable<T> items)
        {
            var result = false;

            if (items != default)
            {
                result = items.Any(i => !i.IsDefault());
            }

            return result;
        }

        public static IEnumerable<T> IfAny<T>(this IEnumerable<T> items)
        {
            if (items != default)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> IfAnyNonDefault<T>(this IEnumerable<T> items)
        {
            if (items?.AnyItemNonDefault() == true)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> NonDefaults<T>(this IEnumerable<T> items)
        {
            var relevants = items?
                .Where(i => !i.IsDefault());

            if (relevants != default)
            {
                foreach (var relevant in relevants)
                {
                    yield return relevant;
                }
            }
        }

        #endregion Public Methods
    }
}