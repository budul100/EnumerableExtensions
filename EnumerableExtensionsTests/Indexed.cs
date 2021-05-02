using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EnumerableExtensionsTests
{
    internal class Indexed
        : Base

    {
        #region Public Methods

        [Test]
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

        [Test]
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

        [Test]
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

            Assert.True(result == 4);
        }

        #endregion Public Methods
    }
}