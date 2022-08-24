using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EnumerableExtensionsTests
{
    internal class Convert
        : Base
    {
        #region Public Methods

        [Test]
        public void ToArrayOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToArrayOrDefault() == default);
            Assert.IsTrue(valuesEmpty.ToArrayOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToArrayOrDefault() == default);
        }

        [Test]
        public void ToArrayOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToArrayOrDefault();

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToListOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToListOrDefault() == default);
            Assert.IsTrue(valuesEmpty.ToListOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToListOrDefault() == default);
        }

        [Test]
        public void ToListOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToListOrDefault();

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}