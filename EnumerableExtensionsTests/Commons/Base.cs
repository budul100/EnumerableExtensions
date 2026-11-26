using System;
using System.Collections.Generic;

namespace EnumerableExtensionsTests.Commons
{
    public abstract class Base
    {
        #region Protected Fields

        /// <summary>
        /// Number of enumerations of GetWithDisposal which were disposed (completed or abandoned).
        /// An enumeration which is never disposed does not count.
        /// </summary>
        protected int disposalCount;

        #endregion Protected Fields

        #region Protected Methods

        protected IEnumerable<T> GetWithDisposal<T>(params T[] datas)
        {
            ArgumentNullException.ThrowIfNull(datas);

            try
            {
                foreach (var data in datas)
                {
                    yield return data;
                }
            }
            finally
            {
                disposalCount++;
            }
        }

        #endregion Protected Methods
    }
}
