using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    internal class Chain
        : Base
    {
        #region Public Methods

        [Test]
        public void SkipLast()
        {
            var result = Extensions.SkipLast(GetWithDisposal("a", "b", "c"), 2);

            Assert.IsTrue(result.Count() == 1);
        }

        [Test]
        public void SkipLastTooMuch()
        {
            var result = Extensions.SkipLast(GetWithDisposal("a", "b", "c"), 4);

            Assert.IsFalse(result.Any());
        }

        [Test]
        public void ToConsecutivePairs()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .ToConsecutivePairs((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Length == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToConsecutiveTriples()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c", "d")
                .ToConsecutiveTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.IsTrue(result.Length == 4);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToPairs()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .ToPairs((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Length == 2);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToTriples()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c", "d")
                .ToTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.IsTrue(result.Length == 2);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}