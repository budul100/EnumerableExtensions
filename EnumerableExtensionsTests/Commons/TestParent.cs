using System.Collections.Generic;

namespace EnumerableExtensionsTests.Commons
{
    internal sealed class TestParent(IEnumerable<TestObject> testObjects)
    {
        #region Public Properties

        public IEnumerable<TestObject> TestObjects { get; set; } = testObjects;

        #endregion Public Properties
    }
}
