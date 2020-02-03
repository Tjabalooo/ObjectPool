using System;
using System.Collections.Generic;

namespace ObjectPool.Lib
{
    public class ObjectPool<T> : IDisposable where T : PoolObject<T>, new()
    {
        private bool disposedValue = false; // To detect redundant calls

        private int poolSize;
        private List<T> pool;
        private T nextObj;

        public ObjectPool(int poolSize)
        {
            this.poolSize = poolSize;
            pool = new List<T>(poolSize);
            for (var i = 0; i < poolSize; i++)
            {
                var obj = new T();
                pool.Add(obj);
                if (i > 0)
                    obj.NextObject = pool[i - 1];
            }

            nextObj = pool[poolSize - 1];
        }

        public int ObjectsLeft 
        { 
            get
            {
                return poolSize;
            }
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                var nextObjectNullCount = 0;
                foreach (var obj in pool)
                {
                    if (obj.NextObject == null)
                        nextObjectNullCount++;

                    obj.Dispose();
                }

                if (nextObjectNullCount != 1)
                    throw new ObjectPoolStillInUseException();

                disposedValue = true;
            }
        }

        public T GetObjectFromPool()
        {
            if (poolSize == 0)
                throw new PoolSizeExceededException();

            poolSize--;

            var obj = nextObj;
            nextObj = obj.NextObject;
            obj.NextObject = null;

            return obj;
        }

        public void ReturnObjectToPool(ref T poolObject)
        {
            poolObject.Initialize();

            poolObject.NextObject = nextObj;
            nextObj = poolObject;
            poolObject = null;

            poolSize++;
        }
    }
}
