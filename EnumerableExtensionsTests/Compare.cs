using System;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests;

public class Compare
    : Base
{
    #region Private Fields

    private static readonly int[] current = [1];
    private static readonly int[] currentArray = [1, 2];

    #endregion Private Fields

    #region Public Methods

    [Fact]
    public void ContainsOrEmpty()
    {
        var element = new TestObject(1);

        Assert.True(GetWithDisposal(default, element, default).ContainsOrEmpty(element));
        Assert.True(Array.Empty<TestObject>().ContainsOrEmpty(element));
    }

    [Fact]
    public void ContainsOrEmptyEnumeratesOnce()
    {
        disposalCount = 0;

        Assert.False(GetWithDisposal(1, 2, 3).ContainsOrEmpty(4));

        Assert.Equal(1, disposalCount);
    }

    [Fact]
    public void EqualsOrDefaultWithNull()
    {
        Assert.True(((int[])null).EqualsOrDefault([1]));
        Assert.False(current.EqualsOrDefault(null));
        Assert.True(currentArray.EqualsOrDefault([2, 1]));
    }

    #endregion Public Methods
}
