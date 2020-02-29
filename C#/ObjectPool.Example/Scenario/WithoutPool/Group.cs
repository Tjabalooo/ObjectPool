using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Example.Scenario.WithoutPool
{
    internal class Group : IGroup
    {
        public Group()
        {
            Things = new List<IThing>();
        }

        public List<IThing> Things { get; }
    }
}
