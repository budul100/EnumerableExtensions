using System;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Count
        : Base
    {
        #region Public Methods

        [Fact]
        public void MaxOrDefault()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.True(valuesDefaultInt.MaxOrDefault() == default);
            Assert.True(valuesDefaultDateTime.MaxOrDefault() == default);
            Assert.Equal(3, GetWithDisposal(3, 1, 2).MaxOrDefault());
        }

        [Fact]
        public void MaxOrNull()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.True(valuesDefaultInt.MaxOrNull() == default);
            Assert.True(valuesDefaultDateTime.MaxOrNull() == default);
            Assert.Equal(3, GetWithDisposal(3, 1, 2).MaxOrNull());
        }

        [Fact]
        public void MinOrDefault()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.True(valuesDefaultInt.MinOrDefault() == default);
            Assert.True(valuesDefaultDateTime.MinOrDefault() == default);
            Assert.Equal(1, GetWithDisposal(3, 1, 2).MinOrDefault());
        }

        [Fact]
        public void MinOrNull()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.True(valuesDefaultInt.MinOrNull() == default);
            Assert.True(valuesDefaultDateTime.MinOrNull() == default);
            Assert.Equal(1, GetWithDisposal(3, 1, 2).MinOrNull());
        }

        #endregion Public Methods
    }
}