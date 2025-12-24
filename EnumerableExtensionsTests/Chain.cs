using System.Linq;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Chain
        : Base
    {
        #region Public Methods

        [Fact]
        public void DistinctSuccessive()
        {
            var a = new object();
            var b = new object();

            var x = GetWithDisposal(b, b, a, a, b, b);

            var result = x.DistinctSuccessive();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void DistinctSuccessiveNull()
        {
            var x = GetWithDisposal("a", "a", default, default, "a", "a");

            var result = x.DistinctSuccessive();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void ToConsecutivePairs()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .ToConsecutivePairs((x, y) => new { x, y }).ToArray();

            Assert.Equal(4, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToConsecutiveTriples()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c", "d")
                .ToConsecutiveTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.Equal(6, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToConsecutiveTriplesWithTooLess()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b")
                .ToConsecutiveTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.Equal(4, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToPairs()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .ToPairs((x, y) => new { x, y }).ToArray();

            Assert.Equal(2, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToTriples()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c", "d")
                .ToTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.Equal(2, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void ToTriplesWithTooLess()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b")
                .ToTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.Empty(result);

            Assert.Equal(1, disposalCount);
        }

        #endregion Public Methods
    }
}