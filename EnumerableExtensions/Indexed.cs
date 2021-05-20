using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static int Count(this System.Collections.IEnumerable items)
        {
            var result = 0;

            if (items != default)
            {
                var enumerator = items.GetEnumerator();
                result = enumerator.Count();
            }

            return result;
        }

        public static bool CountEqualsOrSingle(this System.Collections.IEnumerable items, params System.Collections.IEnumerable[] others)
        {
            var enumerators = items
                .GetEnumerators(others).ToArray();

            var all = 0;

            foreach (var enumerator in enumerators)
            {
                var current = enumerator.Count();

                if (current > 1)
                {
                    if (all == 0)
                    {
                        all = current;
                    }
                    else if (current != all)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static T ElementAtOrSingle<T>(this IEnumerable<T> items, int index)
        {
            var result = default(T);

            if (items?.Any() ?? false)
            {
                result = items.Count() == 1
                    ? items.Single()
                    : items.ElementAt(index);
            }

            return result;
        }

        public static IEnumerable<int> Indexes(this System.Collections.IEnumerable items, params System.Collections.IEnumerable[] others)
        {
            var enumerators = items
                .GetEnumerators(others).ToArray();

            var length = enumerators.CountMax();

            for (var index = 0; index < length; index++)
            {
                yield return index;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static int Count(this System.Collections.IEnumerator enumerator)
        {
            var result = 0;

            if (enumerator != default)
            {
                while (enumerator.MoveNext())
                {
                    result++;
                }
            }

            return result;
        }

        private static int CountMax(this IEnumerable<System.Collections.IEnumerator> enumerators)
        {
            var result = 0;

            foreach (var enumerator in enumerators)
            {
                int current = Count(enumerator);

                if (current > result)
                {
                    result = current;
                }
            }

            return result;
        }

        private static IEnumerable<System.Collections.IEnumerator> GetEnumerators(this System.Collections.IEnumerable items,
            IEnumerable<System.Collections.IEnumerable> others)
        {
            if (items != default)
            {
                yield return items.GetEnumerator();
            }

            foreach (var other in others.IfAny())
            {
                if (other != default)
                {
                    yield return other.GetEnumerator();
                }
            }
        }

        #endregion Private Methods
    }
}