using System;
using System.Collections.Generic;
using System.Text;
using ObjectPool.Lib;

namespace ObjectPool.Example.Scenario.WithPool
{
    internal class Thing : PoolObject<Thing>, IThing
    {
        public int Value { get; set; }

        public override void DisposeManagedState()
        {
            // nothing to clean
        }

        public override void Initialize()
        {
            Value = 0;
        }
    }
}
