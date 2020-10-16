using EnumerableExtensions;
using NUnit.Framework;
using System;
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
            Assert.IsFalse(GetWithDisposal<TestObject>(default, default, default).AnyNonDefaultItem());
        }

        [Test]
        public void AnyNonDefaultTrue()
        {
            Assert.IsTrue(GetWithDisposal(default, new TestObject(1), default).AnyNonDefaultItem());
        }

        [Test]
        public void Consecutive()
        {
            var result = GetWithDisposal("a", "b", "c")
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
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MaxOrDefault() == default);
            Assert.IsTrue(valuesDefaultDateTime.MaxOrDefault() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MaxOrDefault() == 3);
        }

        [Test]
        public void MergeDefault()
        {
            var result = GetWithDisposal<string>(default, default, default).Merge();

            Assert.IsTrue(result == default);
        }

        [Test]
        public void MergeNonDefault()
        {
            var result = GetWithDisposal(default, default, "c").Merge();

            Assert.IsTrue(result != default);
        }

        [Test]
        public void MinOrDefault()
        {
            var valuesDefaultInt = Array.Empty<int>();
            var valuesDefaultDateTime = Array.Empty<DateTime>();

            Assert.IsTrue(valuesDefaultInt.MinOrDefault() == default);
            Assert.IsTrue(valuesDefaultDateTime.MinOrDefault() == default);
            Assert.IsTrue(GetWithDisposal(3, 1, 2).MinOrDefault() == 1);
        }

        [Test]
        public void NonDefaults()
        {
            Assert.IsTrue(GetWithDisposal(default, new TestObject(1), default).NonDefaults().Count() == 1);
        }

        [Test]
        public void Paired()
        {
            var result = GetWithDisposal("a", "b", "c")
                .Paired((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 2);
        }

        [Test]
        public void SplitAtChange()
        {
            var result = GetWithDisposal(new TestObject(1), new TestObject(1), new TestObject(2), new TestObject(1))
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void ToArrayOrDefault()
        {
            var valuesDefault = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToArrayOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToArrayOrDefault() == default);
        }

        [Test]
        public void ToArrayOrDefaultWithDisposal()
        {
            GetWithDisposal("a", "b", "c").ToArrayOrDefault();
        }

        [Test]
        public void ToListOrDefault()
        {
            var valuesDefault = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToListOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToListOrDefault() == default);
        }

        [Test]
        public void ToListOrDefaultWithDisposal()
        {
            GetWithDisposal("a", "b", "c").ToListOrDefault();
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<T> GetWithDisposal<T>(params T[] datas)
        {
            if (datas == default)
            {
                throw new ArgumentNullException(nameof(datas));
            }
            using (var disposalClass = new DisposalClass())
            {
                foreach (var data in datas)
                {
                    yield return data;
                }

                disposalClass.CanBeDisposed();
            }
        }

        #endregion Private Methods

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