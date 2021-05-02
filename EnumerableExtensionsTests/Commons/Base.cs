using System;
using System.Collections.Generic;

namespace EnumerableExtensionsTests.Commons
{
    public abstract class Base
    {
        #region Protected Fields

        protected int disposalCount;

        #endregion Protected Fields

        #region Protected Methods

        protected IEnumerable<T> GetWithDisposal<T>(params T[] datas)
        {
            disposalCount++;

            if (datas == default)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            using (var disposalClass = new Disposable())
            {
                foreach (var data in datas)
                {
                    yield return data;
                }

                disposalClass.CanBeDisposed();
            }
        }

        #endregion Protected Methods

        #region Protected Classes

        protected class TestObject
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

        protected class TestParent
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

        #endregion Protected Classes
    }
}