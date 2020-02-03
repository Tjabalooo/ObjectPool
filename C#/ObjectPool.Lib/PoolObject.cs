using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Lib
{
    public abstract class PoolObject<T> : IDisposable where T : PoolObject<T>, new()
    {
        private bool disposedValue = false; // To detect redundant calls

        public PoolObject()
        {
            this.Initialize();
        }

        public abstract void Initialize();

        public abstract void DisposeManagedState();

        internal T NextObject { get; set; }

        public void Dispose()
        {
            if (!disposedValue)
            {
                this.NextObject = null;
                this.DisposeManagedState();

                disposedValue = true;
            }
        }
    }
}
