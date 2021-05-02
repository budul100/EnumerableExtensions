using System.Collections.Generic;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Private Methods

        private static bool IsDefault<T>(this T item)
        {
            return item.IsEqual(default);
        }

        private static bool IsEqual<T>(this T current, T other)
        {
            return EqualityComparer<T>.Default.Equals(
                x: current,
                y: other);
        }

        #endregion Private Methods
    }
}