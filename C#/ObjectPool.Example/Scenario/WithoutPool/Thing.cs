using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Example.Scenario.WithoutPool
{
    internal class Thing : IThing
    {
        public int Value { get; set; }
    }
}
