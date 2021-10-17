using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CoffeeMorningAssigner.Models
{
    public class WeekAssignment
    {
        [JsonConstructor]
        public WeekAssignment(string name)
        {
            // The week id will be set when it's added to the history
            WeekId = 0;
            Name = name;
            Groups = new List<Group>();
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
            return Groups.Aggregate(Environment.NewLine, (current, @group) => current + (@group + Environment.NewLine));
        }
    }
}