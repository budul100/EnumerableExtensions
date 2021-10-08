﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static string Merge(string delimiter, params string[] items)
        {
            var result = items.Merge(
                delimiter: delimiter,
                preventSorting: true);

            return result;
        }

        public static string Merge<T, TProp>(this IEnumerable<T> items, Func<T, TProp> property, string delimiter = ",",
            bool preventSorting = false)
        {
            var result = default(string);

            if (items.AnyItemNonDefault())
            {
                result = items
                    .Select(i => property?.Invoke(i)?.ToString())
                    .Merge(
                        delimiter: delimiter,
                        preventSorting: preventSorting);
            }

            return result;
        }

        public static string Merge<T>(this IEnumerable<T> items, string delimiter = ",", bool preventSorting = false)
        {
            var result = default(string);

            var relevants = items
                .Where(i => !i.IsDefault())
                .Where(i => !string.IsNullOrWhiteSpace(i.ToString()))
                .Distinct().ToArray();

            if (relevants.Any())
            {
                if (!preventSorting)
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