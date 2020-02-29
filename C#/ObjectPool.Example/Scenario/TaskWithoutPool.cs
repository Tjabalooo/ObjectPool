using System;
using System.Collections.Generic;
using System.Text;
using ObjectPool.Example.Scenario.WithoutPool;

namespace ObjectPool.Example.Scenario
{
    internal class TaskWithoutPool : IMeasureableTask
    {
        public MeasurableTaskData DoTheTask()
        {
            var data = new MeasurableTaskData { MillisecondsSpentOnTask = 0, SummaryOfCompletedTask = "" };
            long millisecAtStart = 0;
            long sum = 0;

            for (var i = 0; i < Constants.NumberOfIterationsToRun; i++)
            {
                millisecAtStart = DateTime.Now.Millisecond;

                var theCommonTask = new CommonTask<Group, Thing>(() => new Group(), () => new Thing());
                sum += theCommonTask.Run();

                data.MillisecondsSpentOnTask += (DateTime.Now.Millisecond - millisecAtStart);
            }

            data.SummaryOfCompletedTask = 
                "Completed \"The Common Task\" witout any pool!\n" +
                "\n" +
                $"Number of times the task where run: {Constants.NumberOfIterationsToRun}" +
                "\n" +
                $"This run ended with a total sum of: {sum}";

            return data;
        }
    }
}
