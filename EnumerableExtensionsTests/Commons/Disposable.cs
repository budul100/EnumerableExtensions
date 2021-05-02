using System;

namespace EnumerableExtensionsTests
{
    internal class Disposable
        : IDisposable
    {
        #region Private Fields

        private bool canBeDisposed;
        private bool isDisposed;

        #endregion Private Fields

        #region Public Methods

        public void CanBeDisposed()
        {
            canBeDisposed = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!canBeDisposed)
            {
                throw new Exception("Disposed too early");
            }

            if (!isDisposed)
            {
                if (disposing)
                {
                }

                isDisposed = true;
            }
        }

        #endregion Protected Methods
    }
}