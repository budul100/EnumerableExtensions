using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static IEnumerable<TNew> Consecutive<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
        {
            if (getter == default)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (items != default)
            {
                var previous = default(T);

                using (var enumerator = items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (!previous.IsDefault())
                        {
                            yield return getter.Invoke(
                                arg1: previous,
                                arg2: enumerator.Current);
                        }

                        previous = enumerator.Current;
                    }

                    yield return getter.Invoke(
                        arg1: previous,
                        arg2: default);
                }
            }
        }

        public static IEnumerable<T> DistinctSuccessive<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = default)
        {
            if (items != default)
            {
                bool equals(T left, T right) => comparer == default
                  ? left.Equals(right)
                  : comparer.Equals(left, right);

                var first = true;
                var prior = default(T);

                foreach (var item in items)
                {
                    var isDifferent = first || !equals(
                        left: item,
                        right: prior);

                    if (isDifferent)
                        yield return item;

                    first = false;
                    prior = item;
                }
            }
        }

        public static IEnumerable<TNew> Paired<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
        {
            if (getter == default)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (items != default)
            {
                using (var enumerator = items.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }

                    var previous = enumerator.Current;
                    while (enumerator.MoveNext())
                    {
                        yield return getter(
                            arg1: previous,
                            arg2: enumerator.Current);

                        previous = enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> items, int number = 1)
        {
            if (items != default)
            {
                var length = items.Count();

                if (number > length)
                {
                    number = length;
                }

                for (int index = 0; index < length - number; index++)
                {
                    yield return items.ElementAt(index);
                }
            }
        }

        #endregion Public Methods
    }
}