using System;
using System.Collections.Generic;
using System.Text;
using ObjectPool.Lib;

namespace ObjectPool.Example.Scenario.WithPool
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
