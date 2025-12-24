using System.Linq;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Any
        : Base
    {
        #region Public Methods

        [Fact]
        public void AnyNonDefaultFalse()
        {
            disposalCount = 0;

            Assert.False(GetWithDisposal<TestObject>(
                default,
                default,
                default).AnyItemNonDefault());

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void AnyNonDefaultTrue()
        {
            disposalCount = 0;

            Assert.True(GetWithDisposal(
                default,
                new TestObject(1),
                default).AnyItemNonDefault());

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void IfAny()
        {
            TestParent[] values = default;
            var result = values.IfAny()
                .SelectMany(t => t.TestObjects).IfAny();

            Assert.False(result.Any());
        }

        [Fact]
        public void NonDefaults()
        {
            disposalCount = 0;

            Assert.Single(GetWithDisposal(
                default,
                new TestObject(1),
                default).NonDefaults());

            Assert.Equal(1, disposalCount);
        }

        #endregion Public Methods
    }
}