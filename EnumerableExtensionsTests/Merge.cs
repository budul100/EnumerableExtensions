using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests;

public class Merge
    : Base
{
    #region Private Fields

    private static readonly int[] items = [10, 2];
    private static readonly int[] itemsArray = [2, 0, 1];

    #endregion Private Fields

    #region Public Methods

    [Fact]
    public void MergeDefault()
    {
        disposalCount = 0;

        var result = GetWithDisposal<string>(
            default,
            default,
            default).Merge();

        Assert.True(result == default);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeDistincted()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            "c",
            "b",
            "c").Merge();

        Assert.Equal("b,c", result);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeEmpty()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            default,
            default,
            "").Merge();

        Assert.True(result == default);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeNonDefault()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            default,
            default,
            "c").Merge();

        Assert.True(result != default);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeNumbersAreSortedNumerically()
    {
        Assert.Equal("2,10", items.Merge());
    }

    [Fact]
    public void MergePropertySkipsNullItems()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            new TestObject(2),
            default,
            new TestObject(1))
            .Merge(v => v.Value1);

        Assert.Equal("1,2", result);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeSorted()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            "c",
            "b",
            "a").Merge();

        Assert.Equal("a,b,c", result);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeUndistincted()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            "c",
            "b",
            "c").Merge(preventDistinct: true);

        Assert.Equal("b,c,c", result);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeUnsorted()
    {
        disposalCount = 0;

        var result = GetWithDisposal(
            "c",
            "b",
            "a").Merge(preventSort: true);

        Assert.Equal("c,b,a", result);

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void MergeValueTypesKeepDefaultValues()
    {
        Assert.Equal("0,1,2", itemsArray.Merge());
    }

    #endregion Public Methods
}
