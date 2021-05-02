using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System;

namespace EnumerableExtensionsTests
{
    internal class Count
        : Base
    {
        #region Public Methods

        [Test]
        public void MaxOrDefault()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MaxOrDefault() == default);
            Assert.IsTrue(valuesDefaultDateTime.MaxOrDefault() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MaxOrDefault() == 3);
        }

        [Test]
        public void MaxOrNull()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MaxOrNull() == default);
            Assert.IsTrue(valuesDefaultDateTime.MaxOrNull() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MaxOrNull() == 3);
        }

        [Test]
        public void MinOrDefault()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MinOrDefault() == default);
            Assert.IsTrue(valuesDefaultDateTime.MinOrDefault() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MinOrDefault() == 1);
        }

        [Test]
        public void MinOrNull()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MinOrNull() == default);
            Assert.IsTrue(valuesDefaultDateTime.MinOrNull() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MinOrNull() == 1);
        }

        #endregion Public Methods
    }
}