using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool.Example.Scenario
{
    internal class CommonTask<Group, Thing> where Group : IGroup where Thing : IThing
    {
        private Func<Group> createGroup;
        private Func<Thing> createThing;
        private Action<Group> deleteGroup;

        public CommonTask(Func<Group> createGroup, Func<Thing> createThing,
            Action<Group> deleteGroup = null)
        {
            this.createGroup = createGroup;
            this.createThing = createThing;
            this.deleteGroup = deleteGroup ?? new Action<Group>((g) => { });
        }

        public long Run()
        {
            var group = createGroup();

            var randomizer = new Random();

            for (var i = 0; i < 20; i++)
            {
                var thing = createThing();
                thing.Value = (int)Math.Floor(randomizer.NextDouble() * 101);
                group.Things.Add(thing);
            }

            var sum = group.Things.Sum((t) => t.Value);

            deleteGroup(group);

            return sum;
        }
    }
}
