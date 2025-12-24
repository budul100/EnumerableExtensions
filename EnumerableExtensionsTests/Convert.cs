using System;
using System.Collections.Generic;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Convert
        : Base
    {
        #region Public Methods

        [Fact]
        public void ToArrayOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.True(valuesDefault.ToArrayOrDefault() == default);
            Assert.True(valuesEmpty.ToArrayOrDefault() == default);
            Assert.False(valuesNonDefault.ToArrayOrDefault() == default);
        }

        [Fact]
        public void ToArrayOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToArrayOrDefault();

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToListOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.True(valuesDefault.ToListOrDefault() == default);
            Assert.True(valuesEmpty.ToListOrDefault() == default);
            Assert.False(valuesNonDefault.ToListOrDefault() == default);
        }

        [Fact]
        public void ToListOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToListOrDefault();

            Assert.Equal(1, disposalCount);
        }

        #endregion Public Methods
    }
}