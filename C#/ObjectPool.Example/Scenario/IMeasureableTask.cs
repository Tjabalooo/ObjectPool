using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectPool.Example.Scenario
{
    internal struct MeasurableTaskData
    {
        public long MillisecondsSpentOnTask;
        public string SummaryOfCompletedTask;
    }

    internal interface IMeasureableTask
    {
        MeasurableTaskData DoTheTask();
    }
}
