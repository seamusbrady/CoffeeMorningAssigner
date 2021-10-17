using System.Collections.Generic;
using System.Linq;
using CoffeeMorningAssigner.Models;
using GAF;

namespace CoffeeMorningAssigner
{
    public class Converter
    {
        public static WeekAssignment ConvertToWeekAssignment(Chromosome chromosome, string weekName)
        {
            var assignment = new WeekAssignment(weekName);

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

                assignment.Add(group);
            }

            return assignment;
        }

        public static List<UserAssignment> ConvertToUserAssignments(WeekAssignment weekAssignment, List<User> excludedUsers)
        {
            var userAssignments = new List<UserAssignment>();
            foreach (var group in weekAssignment.Groups)
            {
                foreach (var user in group.Members)
                {
                    userAssignments.Add(new UserAssignment(user, group.Id));
                }
            }

            foreach (var excludedUser in excludedUsers)
            {
                userAssignments.Add(new UserAssignment(excludedUser, null));
            }

            return userAssignments
                .OrderBy(u => u.Exclude.GetValueOrDefault(false))
                .ThenBy(u => u.GroupId)
                .ThenBy(u => u.Name)
                .ToList();
        }

    }
}