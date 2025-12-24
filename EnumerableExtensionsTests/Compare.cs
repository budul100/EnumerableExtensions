using System;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Compare
        : Base
    {
        #region Public Methods

        [Fact]
        public void ContainsOrEmpty()
        {
            var element = new TestObject(1);

            Assert.True(GetWithDisposal(default, element, default).ContainsOrEmpty(element));
            Assert.True(Array.Empty<TestObject>().ContainsOrEmpty(element));
        }

        #endregion Public Methods
    }
}