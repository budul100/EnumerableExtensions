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
        public void DistinctSuccessive()
        {
            var a = new object();
            var b = new object();

            var x = GetWithDisposal(b, b, a, a, b, b);

            var result = x.DistinctSuccessive();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void DistinctSuccessiveNull()
        {
            var x = GetWithDisposal("a", "a", default, default, "a", "a");

            var result = x.DistinctSuccessive();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void ToConsecutivePairs()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .ToConsecutivePairs((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Length == 4);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToConsecutiveTriples()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c", "d")
                .ToConsecutiveTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.IsTrue(result.Length == 6);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToConsecutiveTriplesWithTooLess()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b")
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

        [Test]
        public void ToTriplesWithTooLess()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b")
                .ToTriples((x, y, z) => new { x, y, z }).ToArray();

            Assert.IsTrue(result.Length == 0);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}