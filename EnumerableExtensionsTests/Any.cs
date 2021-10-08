using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    internal class Any
        : Base
    {
        #region Public Methods

        [Test]
        public void AnyNonDefaultFalse()
        {
            disposalCount = 0;

            Assert.IsFalse(GetWithDisposal<TestObject>(
                default,
                default,
                default).AnyItemNonDefault());

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void AnyNonDefaultTrue()
        {
            disposalCount = 0;

            Assert.IsTrue(GetWithDisposal(
                default,
                new TestObject(1),
                default).AnyItemNonDefault());

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void IfAny()
        {
            TestParent[] values = default;
            var result = values.IfAny()
                .SelectMany(t => t.TestObjects).IfAny();

            Assert.IsFalse(result.Any());
        }

        [Test]
        public void NonDefaults()
        {
            disposalCount = 0;

            Assert.IsTrue(GetWithDisposal(
                default,
                new TestObject(1),
                default).NonDefaults().Count() == 1);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}