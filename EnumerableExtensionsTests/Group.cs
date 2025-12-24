using System.Linq;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Group
        : Base
    {
        #region Public Methods

        [Fact]
        public void Framed()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.Equal(2, result.Length);
            Assert.Equal(2, result[0].Count());
            Assert.Equal(3, result[1].Count());

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void FramedWithoutEnd()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(2))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.Empty(result);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void FramedWithoutStart()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(2),
                new TestObject(1))
                .Framed(v => v.Value1 == 1).ToArray();

            Assert.Empty(result);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void SpliAt()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .SplitAt(v => v.Value1 == 1).ToArray();

            Assert.Equal(2, result.Length);
            Assert.Equal(2, result[0].Count());
            Assert.Equal(3, result[1].Count());

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void SpliAtSingle()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(2))
                .SplitAt(v => v.Value1 == 1).ToArray();

            Assert.Single(result);
            Assert.Single(result[0]);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void SpliAtWithoutStart()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(2),
                new TestObject(1),
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .SplitAt(v => v.Value1 == 1).ToArray();

            Assert.Equal(4, result.Length);
            Assert.Equal(2, result[0].Count());
            Assert.Equal(2, result[1].Count());
            Assert.Equal(2, result[2].Count());
            Assert.Equal(3, result[3].Count());

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void SplitAtChange()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1))
                .SplitAtChange(v => v.Value1).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(1, disposalCount);
        }

        [Fact]
        public void SplitAtWithoutEnd()
        {
            disposalCount = 0;

            var result = GetWithDisposal(
                new TestObject(1),
                new TestObject(1),
                new TestObject(2),
                new TestObject(1),
                new TestObject(1),
                new TestObject(2))
                .SplitAt(v => v.Value1 == 1).ToArray();

            Assert.Equal(4, result.Length);
            Assert.Equal(2, result[0].Count());
            Assert.Equal(3, result[1].Count());
            Assert.Equal(2, result[2].Count());
            Assert.Equal(2, result[2].Count());

            Assert.Equal(1, disposalCount);
        }

        #endregion Public Methods
    }
}