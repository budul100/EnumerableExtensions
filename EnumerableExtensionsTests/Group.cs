using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    internal class Group
        : Base
    {
        #region Public Methods

        [Test]
        public void Chunked()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .Chunked(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result[0].Count() == 2);
            Assert.IsTrue(result[1].Count() == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ChunkedWithoutEnd()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1),
                new TestObject(1),
                new TestObject(2))
                .Chunked(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[0].Count() == 2);
            Assert.IsTrue(result[1].Count() == 3);
            Assert.IsTrue(result[2].Count() == 2);
            Assert.IsTrue(result[2].Count() == 2);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void ChunkedWithoutStart()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(2),
                new TestObject(1),
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .Chunked(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[0].Count() == 2);
            Assert.IsTrue(result[1].Count() == 2);
            Assert.IsTrue(result[2].Count() == 2);
            Assert.IsTrue(result[3].Count() == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void Framed()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result[0].Count() == 2);
            Assert.IsTrue(result[1].Count() == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void FramedWithoutEnd()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(2))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 0);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void FramedWithoutStart()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(2),
                new TestObject(1))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.IsTrue(result.Length == 0);

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

            Assert.IsTrue(result.Length == 3);

            Assert.True(disposalCount == 1);
        }

        #endregion Public Methods
    }
}