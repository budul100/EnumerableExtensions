using System;
using System.Collections.Generic;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Indexed
        : Base
    {
        #region Public Methods

        [Fact]
        public void Count()
        {
            var values1 = GetWithDisposal(DateTime.Today);
            var values2 = GetWithDisposal("a", "b", "c", "d");
            var values3 = default(IEnumerable<object>);

            Assert.Equal(1, values1.Count());
            Assert.Equal(4, values2.Count());
            Assert.Equal(0, values3.Count());
        }

        [Fact]
        public void CountEqualsOrSingleFalse()
        {
            var values1 = GetWithDisposal(DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(-11));
            var values2 = Array.Empty<int>();
            var values3 = GetWithDisposal("a", "b", "c", "d");
            var values4 = default(IEnumerable<object>);

            var result = values1.CountEqualsOrSingle(
                values2,
                values3,
                values4);

            Assert.False(result);
        }

        [Fact]
        public void CountEqualsOrSingleTrue()
        {
            var values1 = GetWithDisposal(DateTime.Today);
            var values2 = Array.Empty<int>();
            var values3 = GetWithDisposal("a", "b", "c", "d");
            var values4 = default(IEnumerable<object>);

            var result = values1.CountEqualsOrSingle(
                values2,
                values3,
                values4);

            Assert.True(result);
        }

        [Fact]
        public void Indexes()
        {
            var values1 = GetWithDisposal(DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(-11));
            var values2 = Array.Empty<int>();
            var values3 = GetWithDisposal("a", "b", "c", "d");
            var values4 = default(IEnumerable<object>);

            var indexes = values1.Indexes(
                values2,
                values3,
                values4);

            var result = 0;
            foreach (var index in indexes)
            {
                result++;
            }

            Assert.Equal(4, result);
        }

        #endregion Public Methods
    }
}