using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    internal class Group
        : Base
    {
        #region Public Methods

        [Test]
        public void SplitAtChange()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.IsTrue(result.Count() == 3);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}