using ObjectPool.Lib;
using System;

namespace ObjectPool.UnitTests
{
    public class DummyObj : PoolObject<DummyObj>
    {
        private Action callbackWhenInitialized;
        private Action callbackWhenDisposed;

        public int ValueThatDoesntChangeOnInitialize { get; internal set; }

        public DummyObj()
        {
            callbackWhenInitialized = () => { };
        }

        internal void SetCallbackToVerifyInitialization(Action callback)
        {
            callbackWhenInitialized = callback;
        }

        public override void Initialize()
        {
            callbackWhenInitialized?.Invoke();
        }

        public override void DisposeManagedState()
        {
            callbackWhenDisposed?.Invoke();
        }

        internal void SetCalbackToVerifyDisposal(Action callback)
        {
            callbackWhenDisposed = callback;
        }
    }
}