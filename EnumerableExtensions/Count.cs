using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static int CountOrDefault<T>(this IEnumerable<T> items)
        {
            var result = 0;

            if (items.AnyItem())
            {
                result = items.Count();
            }

            return result;
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> items)
        {
            var result = default(T);

            if (items.AnyItem())
            {
                result = items.Max();
            }

            return result;
        }

        public static Nullable<T> MaxOrNull<T>(this IEnumerable<T> items)
            where T : struct
        {
            var result = default(Nullable<T>);

            if (items.AnyItem())
            {
                result = items.Max();
            }

            return result;
        }

        public static T MinOrDefault<T>(this IEnumerable<T> items)
        {
            var result = default(T);

            if (items.AnyItem())
            {
                result = items.Min();
            }

            return result;
        }

        public static Nullable<T> MinOrNull<T>(this IEnumerable<T> items)
            where T : struct
        {
            var result = default(Nullable<T>);

            if (items.AnyItem())
            {
                result = items.Min();
            }

            return result;
        }

        #endregion Public Methods
    }
}