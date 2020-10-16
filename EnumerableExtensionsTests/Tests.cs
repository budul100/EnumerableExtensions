using EnumerableExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensionsTests
{
    public class Tests
    {
        #region Private Fields

        private int disposalCount;

        #endregion Private Fields

        #region Public Methods

        [Test]
        public void AnyNonDefaultFalse()
        {
            disposalCount = 0;

            Assert.IsFalse(GetWithDisposal<TestObject>(
                default,
                default,
                default).AnyNonDefaultItem());

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void AnyNonDefaultTrue()
        {
            disposalCount = 0;

            Assert.IsTrue(GetWithDisposal(
                default,
                new TestObject(1),
                default).AnyNonDefaultItem());

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void Consecutive()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .Consecutive((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 3);

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
            disposalCount = 0;

            var result = GetWithDisposal<string>(default, default, default).Merge();

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
            disposalCount = 0;

            Assert.IsTrue(GetWithDisposal(
                default,
                new TestObject(1),
                default).NonDefaults().Count() == 1);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void Paired()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .Paired((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 2);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void SplitAtChange()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.IsTrue(result.Count() == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToArrayOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToArrayOrDefault() == default);
            Assert.IsTrue(valuesEmpty.ToArrayOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToArrayOrDefault() == default);
        }

        [Test]
        public void ToArrayOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToArrayOrDefault();

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ToListOrDefault()
        {
            var valuesDefault = default(IEnumerable<TestObject>);
            var valuesEmpty = Array.Empty<TestObject>();
            var valuesNonDefault = new TestObject[] { default, default };

            Assert.IsTrue(valuesDefault.ToListOrDefault() == default);
            Assert.IsTrue(valuesEmpty.ToListOrDefault() == default);
            Assert.IsFalse(valuesNonDefault.ToListOrDefault() == default);
        }

        [Test]
        public void ToListOrDefaultWithDisposal()
        {
            disposalCount = 0;

            GetWithDisposal("a", "b", "c").ToListOrDefault();

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<T> GetWithDisposal<T>(params T[] datas)
        {
            disposalCount++;

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