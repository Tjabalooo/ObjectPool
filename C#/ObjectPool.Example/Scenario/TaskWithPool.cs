using System;
using System.Collections.Generic;
using System.Text;
using ObjectPool.Example.Scenario.WithPool;
using ObjectPool.Lib;

namespace ObjectPool.Example.Scenario
{
    internal class TaskWithPool : IMeasureableTask
    {
        public MeasurableTaskData DoTheTask()
        {
            var data = new MeasurableTaskData { MillisecondsSpentOnTask = 0, SummaryOfCompletedTask = "" };
            long millisecAtStart = 0;
            long sum = 0;

            var thingPool = new ObjectPool<Thing>(50);

            for (var i = 0; i < Constants.NumberOfIterationsToRun; i++)
            {
                millisecAtStart = DateTime.Now.Millisecond;

                var theCommonTask = new CommonTask<Group, Thing>(
                    () => new Group(), 
                    () => thingPool.GetObjectFromPool(),
                    (group) =>
                    {
                        for (var i = 0; i < group.Things.Count; i++)
                        {
                            var thing = (Thing)group.Things[i];
                            thingPool.ReturnObjectToPool(ref thing);
                        }
                        group.Things.Clear();
                    });

                sum += theCommonTask.Run();

                data.MillisecondsSpentOnTask += (DateTime.Now.Millisecond - millisecAtStart);
            }

            data.SummaryOfCompletedTask = 
                "Completed \"The Common Task\" with a pool for Things!\n" +
                "\n" +
                $"Number of times the task where run: {Constants.NumberOfIterationsToRun}" +
                "\n" +
                $"This run ended with a total sum of: {sum}";

            return data;
        }
    }
}
