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
            if (!PoolObject<T>.IncomingCtorCallFromPool)
                throw new CreatingOutsideOfPoolException();

            this.IncomingDisposeCallFromPool = false;
            this.Initialize();
        }

        public abstract void Initialize();

        public abstract void DisposeManagedState();

        internal T NextObject { get; set; }

        internal bool IncomingDisposeCallFromPool { get; set; }

        private static bool _incomingCtorCallFromPool = false;
        internal static bool IncomingCtorCallFromPool
        {
            get
            {
                var value = _incomingCtorCallFromPool;
                _incomingCtorCallFromPool = false;
                return value;
            }
            set
            {
                _incomingCtorCallFromPool = value;
            }
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                if (!IncomingDisposeCallFromPool)
                    throw new DisposingOutsideOfPoolException();

                this.NextObject = null;
                this.DisposeManagedState();

                disposedValue = true;
            }
        }
    }
}
