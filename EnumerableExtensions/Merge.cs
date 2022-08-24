using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static string Merge(this string delimiter, params string[] items)
        {
            var result = items.Merge(
                delimiter: delimiter,
                preventDistinct: true,
                preventSort: true);

            return result;
        }

        public static string Merge<T, TProp>(this IEnumerable<T> items, Func<T, TProp> property, string delimiter = ",",
            bool preventDistinct = false, bool preventSort = false)
        {
            var result = default(string);

            if (items.AnyItemNonDefault())
            {
                result = items
                    .Select(i => property?.Invoke(i)?.ToString())
                    .Merge(
                        delimiter: delimiter,
                        preventDistinct: preventDistinct,
                        preventSort: preventSort);
            }

            return result;
        }

        public static string Merge<T>(this IEnumerable<T> items, string delimiter = ",", bool preventDistinct = false,
            bool preventSort = false)
        {
            var result = default(string);

            var relevants = items
                .Where(i => !i.IsDefault()
                    && !string.IsNullOrWhiteSpace(i.ToString())).ToArray();

            if (!preventDistinct)
            {
                relevants = relevants
                    .Distinct().ToArray();
            }

            if (relevants.Count() > 0)
            {
                if (!preventSort)
                {
                    relevants = relevants
                        .OrderBy(i => i.ToString()).ToArray();
                }

                result = string.Join(
                    separator: delimiter,
                    values: relevants);
            }

            return result;
        }

        #endregion Public Methods
    }
}