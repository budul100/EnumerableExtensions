using EnumerableExtensions;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    public class Tests
    {
        #region Public Methods

        [Test]
        public void AnyNonDefaultFalse()
        {
            var values = new TestObject[] {
                default,
                default,
                default,
            };

            Assert.IsFalse(values.AnyNonDefaultItem());
        }

        [Test]
        public void AnyNonDefaultTrue()
        {
            var values = new TestObject[] {
                default,
                new TestObject { Value1 = 1 },
                default,
            };

            Assert.IsTrue(values.AnyNonDefaultItem());
        }

        [Test]
        public void SplitAtChange()
        {
            var values = new TestObject[] {
                new TestObject { Value1 = 1 },
                new TestObject { Value1 = 1 },
                new TestObject { Value1 = 2 },
                new TestObject { Value1 = 1 }
            };

            var result = values
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.IsTrue(result.Count() == 3);
        }

        #endregion Public Methods

        #region Private Classes

        private class TestObject
        {
            #region Public Properties

            public int Value1 { get; set; }

            #endregion Public Properties
        }

        #endregion Private Classes
    }
}