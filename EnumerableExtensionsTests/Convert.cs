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
        public void AsArrayOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.AsArrayOrDefault() == default);
            Assert.IsTrue(valuesEmpty.AsArrayOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.AsArrayOrDefault() == default);
        }

        [Test]
        public void AsArrayOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").AsArrayOrDefault();

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void AsListOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.AsListOrDefault() == default);
            Assert.IsTrue(valuesEmpty.AsListOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.AsListOrDefault() == default);
        }

        [Test]
        public void AsListOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").AsListOrDefault();

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}