using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Merge
        : Base
    {
        #region Public Methods

        [Fact]
        public void MergeDefault()
        {
            disposalCount = 0;

            var result = GetWithDisposal<string>(
                default,
                default,
                default).Merge();

            Assert.True(result == default);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeDistincted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "c").Merge();

            Assert.Equal("b,c", result);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeEmpty()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                default,
                default,
                "").Merge();

            Assert.True(result == default);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeNonDefault()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                default,
                default,
                "c").Merge();

            Assert.True(result != default);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeSorted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "a").Merge();

            Assert.Equal("a,b,c", result);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeUndistincted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "c").Merge(preventDistinct: true);

            Assert.Equal("b,c,c", result);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void MergeUnsorted()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                "c",
                "b",
                "a").Merge(preventSort: true);

            Assert.Equal("c,b,a", result);

            Assert.Equal(1, disposalCount);
        }

        #endregion Public Methods
    }
}