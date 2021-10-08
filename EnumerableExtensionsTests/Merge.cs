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

            var result = GetWithDisposal<string>(
                default,
                default,
                default).Merge();

            Assert.IsTrue(result == default);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void MergeDistincted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "c").Merge();

            Assert.IsTrue(result == "b,c");

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void MergeEmpty()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                default,
                default,
                "").Merge();

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

        [Test]
        public void MergeSorted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "a").Merge();

            Assert.IsTrue(result == "a,b,c");

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void MergeUnsorted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "a").Merge(preventSorting: true);

            Assert.IsTrue(result == "c,b,a");

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}