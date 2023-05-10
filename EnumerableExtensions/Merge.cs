using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                .Where(i => !i.IsDefault())
                .Select(i => i.ToString())
                .Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

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
                        .OrderBy(t => t).ToArray();
                }

                var text = new StringBuilder();

                foreach (var relevant in relevants)
                {
                    if (!string.IsNullOrEmpty(delimiter)
                        && text.Length > 0)
                    {
                        text.Append(delimiter);
                    }

                    text.Append(relevant);
                }

                result = text.ToString();
            }

            return result;
        }

        #endregion Public Methods
    }
}