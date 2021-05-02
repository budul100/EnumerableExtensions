using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System;

namespace EnumerableExtensionsTests
{
    internal class Compare
        : Base
    {
        #region Public Methods

        [Test]
        public void ContainsOrEmpty()
        {
            var element = new TestObject(1);

            Assert.IsTrue(GetWithDisposal(default, element, default).ContainsOrEmpty(element));
            Assert.IsTrue(Array.Empty<TestObject>().ContainsOrEmpty(element));
        }

        #endregion Public Methods
    }
}