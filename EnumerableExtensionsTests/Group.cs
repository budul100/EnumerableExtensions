using System;
using System.Linq;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests;

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
    public void GroupByHash()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            new TestObject(1),
            new TestObject(2),
            new TestObject(1))
            .GroupByHash(v => v.Value1).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Equal(2, result[0].Count());
        Assert.Single(result[1]);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void GroupByHashWithoutItems()
    {
        Assert.Empty(((TestObject[])null).GroupByHash(v => v.Value1));
    }

    [Fact]
    public void SplitAt()
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
    public void SplitAtSingle()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            new TestObject(2))
            .SplitAt(v => v.Value1 == 1).ToArray();

        var item = Assert.Single(result);
        Assert.Single(item);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void SplitAtValidatesEagerly()
    {
        // No enumeration, the exception must be thrown by the call itself
        Assert.Throws<ArgumentNullException>(
            () => GetWithDisposal(1, 2).SplitAt(null));
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
        Assert.Equal(2, result[3].Count());

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void SplitAtWithoutStart()
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

    #endregion Public Methods
}
