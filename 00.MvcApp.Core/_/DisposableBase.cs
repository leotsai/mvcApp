using System;

namespace MvcApp.Core
{
    public abstract class DisposableBase : IDisposable
    {
        private bool _disposed = false;
        
        protected abstract void ReleaseUnmanagedResources();

        /// <summary>
        /// Call in ~constructor
        /// </summary>
        protected void MarkDisposed()
        {
            _disposed = true;
        }
        
        public virtual void Dispose()
        {
            if (this._disposed == false)
            {
                this.ReleaseUnmanagedResources();
                this._disposed = true;
            }
            GC.SuppressFinalize(this);
        }

    }
}
