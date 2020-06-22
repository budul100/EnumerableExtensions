using EnumerableExtensions;
using NUnit.Framework;
using System.Collections.Generic;
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
                new TestObject(1),
                default,
            };

            Assert.IsTrue(values.AnyNonDefaultItem());
        }

        [Test]
        public void Consecutive()
        {
            var values = new string[] { "a", "b", "c" };

            var result = values
                .Consecutive((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 3);
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
        public void MaxOrDefault()
        {
            var valuesDefault = System.Array.Empty<int>();
            var valuesNonDefault = new int[] { 3, 1, 2, };

            Assert.IsTrue(valuesDefault.MaxOrDefault() == default);
            Assert.IsTrue(valuesNonDefault.MaxOrDefault() == 3);
        }

        [Test]
        public void MinOrDefault()
        {
            var valuesDefault = System.Array.Empty<int>();
            var valuesNonDefault = new int[] { 3, 1, 2, };

            Assert.IsTrue(valuesDefault.MinOrDefault() == default);
            Assert.IsTrue(valuesNonDefault.MinOrDefault() == 1);
        }

        [Test]
        public void NonDefaults()
        {
            var values = new TestObject[] {
                default,
                new TestObject(1),
                default,
            };

            Assert.IsTrue(values.NonDefaults().Count() == 1);
        }

        [Test]
        public void Paired()
        {
            var values = new string[] { "a", "b", "c" };

            var result = values
                .Paired((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 2);
        }

        [Test]
        public void SplitAtChange()
        {
            var values = new TestObject[] {
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1),
            };

            var result = values
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void ToArrayOrDefault()
        {
            var valuesDefault = System.Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToArrayOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToArrayOrDefault() == default);
        }

        [Test]
        public void ToListOrDefault()
        {
            var valuesDefault = System.Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToListOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToListOrDefault() == default);
        }

        #endregion Public Methods

        #region Private Classes

        private class TestObject
        {
            #region Public Constructors

            public TestObject()
            { }

            public TestObject(int value1)
            {
                Value1 = value1;
            }

            #endregion Public Constructors

            #region Public Properties

            public int Value1 { get; set; }

            #endregion Public Properties
        }

        private class TestParent
        {
            #region Public Constructors

            public TestParent(IEnumerable<TestObject> testObjects)
            {
                TestObjects = testObjects;
            }

            #endregion Public Constructors

            #region Public Properties

            public IEnumerable<TestObject> TestObjects { get; set; }

            #endregion Public Properties
        }

        #endregion Private Classes
    }
}