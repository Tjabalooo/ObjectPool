using ObjectPool.Lib;
using System;
using System.Collections.Generic;
using Xunit;

namespace ObjectPool.UnitTests
{
    public class ObjectPoolTests
    {
        private const int POOL_SIZE = 5;

        private ObjectPool<DummyObj> sut;

        public ObjectPoolTests()
        {
            sut = new ObjectPool<DummyObj>(POOL_SIZE);
        }

        [Fact]
        public void RetrievingOneObjectFromPool()
        {
            var retrievedObj = sut.GetObjectFromPool();

            Assert.Equal(POOL_SIZE - 1, sut.ObjectsLeft);
        }

        [Fact]
        public void ReturningRetrievedObjectToPool()
        {
            var retrievedObj = sut.GetObjectFromPool();
            sut.ReturnObjectToPool(ref retrievedObj);

            Assert.Equal(POOL_SIZE, sut.ObjectsLeft);
        }

        [Fact]
        public void ReturningAnObjectCallsTheInitializeFunction()
        {
            var initialized = false;
            var retrievedObj = sut.GetObjectFromPool();
            retrievedObj.SetCallbackToVerifyInitialization(() => initialized = true);
            sut.ReturnObjectToPool(ref retrievedObj);

            Assert.True(initialized);
        }

        [Fact]
        public void TwoRetrievedObjectsAreUnique()
        {
            var obj1 = sut.GetObjectFromPool();
            var obj2 = sut.GetObjectFromPool();

            obj1.ValueThatDoesntChangeOnInitialize = 1;
            obj2.ValueThatDoesntChangeOnInitialize = 2;

            Assert.False(obj1.ValueThatDoesntChangeOnInitialize == obj2.ValueThatDoesntChangeOnInitialize, "Values of objects should differ!");
        }

        [Fact]
        public void TryingToRetrieveMoreObjectsThenPossibleThrowsException()
        {
            var objects = new List<DummyObj>();
            for (var i = 0; i < POOL_SIZE; i++)
                objects.Add(sut.GetObjectFromPool());

            Assert.Throws<PoolSizeExceededException>(() => objects.Add(sut.GetObjectFromPool()));
        }

        [Fact]
        public void TheSameObjectsAreReusedInThePool()
        {
            var objects = new List<DummyObj>();
            var listOfUniqueValues = new List<int>();

            for (var i = 0; i < POOL_SIZE; i++)
            {
                var obj = sut.GetObjectFromPool();
                var nextValue = i;
                obj.ValueThatDoesntChangeOnInitialize = nextValue;
                objects.Add(obj);
                listOfUniqueValues.Add(nextValue);
            }

            for (var i = 0; i < POOL_SIZE; i++)
            {
                var obj = objects[i];
                sut.ReturnObjectToPool(ref obj);
            }

            objects.Clear();
            for (var i = 0; i < POOL_SIZE; i++)
            {
                var obj = sut.GetObjectFromPool();
                Assert.True(listOfUniqueValues.Contains(obj.ValueThatDoesntChangeOnInitialize), $"Object with value {obj.ValueThatDoesntChangeOnInitialize} is not a reused object!");
                listOfUniqueValues.Remove(obj.ValueThatDoesntChangeOnInitialize);
            }
        }

        [Fact]
        public void DisposingObjectPoolBeforeAllObjectsAreReturnedThrowsException()
        {
            var obj = sut.GetObjectFromPool();

            Assert.Throws<ObjectPoolStillInUseException>(() => sut.Dispose());
        }

        [Fact]
        public void DisposingObjectPoolAfterAllObjectsAreReturnedWorks()
        {
            var obj = sut.GetObjectFromPool();

            sut.ReturnObjectToPool(ref obj);

            sut.Dispose();
        }

        [Fact]
        public void ReturnedObjectRefTurnsToNull()
        {
            var obj = sut.GetObjectFromPool();

            sut.ReturnObjectToPool(ref obj);

            Assert.Null(obj);
        }

        [Fact]
        public void DisposingOfObjectPoolDisposesPoolObjects()
        {
            var objects = new List<DummyObj>();
            var disposeCount = 0;

            for (var i = 0; i < POOL_SIZE; i++)
            {
                var obj = sut.GetObjectFromPool();
                obj.SetCalbackToVerifyDisposal(() => disposeCount++);
                objects.Add(obj);
            }

            for (var i = 0; i < POOL_SIZE; i++)
            {
                var obj = objects[i];
                sut.ReturnObjectToPool(ref obj);
            }

            sut.Dispose();

            Assert.Equal(POOL_SIZE, disposeCount);
        }

        [Fact]
        public void DisposingAnObjectOutsideOfThePoolThrowsException()
        {
            var obj = sut.GetObjectFromPool();

            Assert.Throws<DisposingOutsideOfPoolException>(() => obj.Dispose());
        }

        [Fact]
        public void CreatingAnObjectOutsideOfThePoolThrowsException()
        {
            Assert.Throws<CreatingOutsideOfPoolException>(() => { var obj = new DummyObj(); });
        }
    }
}
