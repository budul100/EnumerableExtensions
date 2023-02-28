using System;
using System.Collections.Generic;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static IEnumerable<T> DistinctSuccessive<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = default)
        {
            if (items != default)
            {
                var first = true;
                var prior = default(T);

                foreach (var item in items)
                {
                    var isDifferent = first || !comparer.Equals(
                        left: item,
                        right: prior);

                    if (isDifferent)
                        yield return item;

                    first = false;
                    prior = item;
                }
            }
        }

        public static IEnumerable<TNew> ToConsecutivePairs<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
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
                        yield return getter.Invoke(
                            arg1: previous,
                            arg2: enumerator.Current);

                        previous = enumerator.Current;
                    }

                    yield return getter.Invoke(
                        arg1: previous,
                        arg2: default);
                }
            }
        }

        public static IEnumerable<TNew> ToConsecutiveTriples<T, TNew>(this IEnumerable<T> items, Func<T, T, T, TNew> getter)
        {
            if (getter == default)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (items != default)
            {
                var previous1 = default(T);
                var previous2 = default(T);

                using (var enumerator = items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        yield return getter(
                            arg1: previous2,
                            arg2: previous1,
                            arg3: enumerator.Current);

                        previous2 = previous1;
                        previous1 = enumerator.Current;
                    }

                    yield return getter(
                        arg1: previous2,
                        arg2: previous1,
                        arg3: default);

                    yield return getter(
                        arg1: previous1,
                        arg2: default,
                        arg3: default);
                }
            }
        }

        public static IEnumerable<TNew> ToPairs<T, TNew>(this IEnumerable<T> items, Func<T, T, TNew> getter)
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

        public static IEnumerable<TNew> ToTriples<T, TNew>(this IEnumerable<T> items, Func<T, T, T, TNew> getter)
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

                    var previous1 = enumerator.Current;

                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }

                    var previous2 = previous1;
                    previous1 = enumerator.Current;

                    while (enumerator.MoveNext())
                    {
                        yield return getter(
                            arg1: previous2,
                            arg2: previous1,
                            arg3: enumerator.Current);

                        previous2 = previous1;
                        previous1 = enumerator.Current;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static bool Equals<T>(this IEqualityComparer<T> comparer, T left, T right)
        {
            if ((left?.Equals(default) ?? true) || (right?.Equals(default) ?? true))
            {
                return (left?.Equals(default) ?? true)
                    && (right?.Equals(default) ?? true);
            }
            else
            {
                return comparer == default
                    ? left.Equals(right)
                    : comparer.Equals(
                        x: left,
                        y: right);
            }
        }

        #endregion Private Methods
    }
}