using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;

namespace EnumerableExtensionsTests
{
    internal class Merge
        : Base
    {
        #region Public Methods

        [Test]
        public void MergeDefault()
        {
            disposalCount = 0;

            var result = GetWithDisposal<string>(default, default, default).Merge();

            Assert.IsTrue(result == default);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void MergeNonDefault()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                default,
                default,
                "c").Merge();

            Assert.IsTrue(result != default);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}