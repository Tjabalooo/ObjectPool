using System;

namespace ObjectPool.Lib
{
    public class PoolSizeExceededException : Exception
    {
    }

    public class ObjectPoolStillInUseException : Exception
    {
    }
}