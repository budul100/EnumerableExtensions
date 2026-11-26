namespace EnumerableExtensionsTests.Commons;

internal sealed class TestObject
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
