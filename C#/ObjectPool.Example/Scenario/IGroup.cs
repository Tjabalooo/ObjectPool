using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Example.Scenario
{
    internal interface IGroup
    {
        List<IThing> Things { get; }
    }
}
