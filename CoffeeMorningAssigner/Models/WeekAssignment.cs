using System;
using System.Collections.Generic;
using System.Linq;
using GAF;

namespace CoffeeMorningAssigner.Models
{
    public class WeekAssignment
    {
        public WeekAssignment(string name)
        {
            // The week id will be set when it's added to the history
            WeekId = 0;
            Name = name;
            Groups = new List<Group>();
        }

        public WeekAssignment(Chromosome chromosome, string weekName) : this(weekName)
        {
            var users = chromosome.Genes
                .Select(gene => (User)gene.ObjectValue)
                .ToList();

            var queue = new Queue<User>(users);

            int groupIndex = 1;
            while (queue.Count > 0)
            {
                var group = new Group(groupIndex++);
                while (!group.IsFull() && queue.Count > 0)
                {
                    group.Add(queue.Dequeue());
                }

                // Add the last user to the current group (even if it exceeds max count) 
                //  to ensure that we don't end up with a user in their own group.
                if (queue.Count == 1)
                {
                    group.Add(queue.Dequeue());
                }

                Add(group);
            }
        }

        public int WeekId { get; set; }
        public string Name { get; set; }
        public List<Group> Groups { get; }

        public void Add(Group group)
        {
            Groups.Add(group);
        }


        public int UserCount
        {
            get { return Groups.Sum(p => p.Members.Count); }
        }

        public override string ToString()
        {
            var heading = Environment.NewLine + "This week's groupings" + Environment.NewLine;
            return Groups.Aggregate(heading, (current, @group) => current + (@group + Environment.NewLine));
        }
    }
}