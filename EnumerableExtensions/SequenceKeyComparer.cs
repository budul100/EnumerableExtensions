using System;
using System.Collections.Generic;

namespace EnumerableExtensions;

internal sealed class SequenceKeyComparer<T>
    : IEqualityComparer<T[]>
{
    #region Public Methods

    public bool Equals(T[] x, T[] y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null
            || y is null
            || x.Length != y.Length)
        {
            return false;
        }

        var comparer = EqualityComparer<T>.Default;

        for (var index = 0; index < x.Length; index++)
        {
            if (!comparer.Equals(x[index], y[index]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(T[] obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var comparer = EqualityComparer<T>.Default;
        var result = new HashCode();

        foreach (var item in obj)
        {
            result.Add(
                value: item,
                comparer: comparer);
        }

        return result.ToHashCode();
    }

    #endregion Public Methods
}
