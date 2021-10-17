using System.Collections.Generic;
using System.Linq;

namespace CoffeeMorningAssigner.Models
{

    public class AssignmentHistory
    {
        public AssignmentHistory()
        {
            Weeks = new List<WeekAssignment>();
        }

        public List<WeekAssignment> Weeks { get; }

        public void Add(WeekAssignment weekAssignment)
        {
            var maxWeekId = Weeks.Any() ? Weeks.Max(w => w.WeekId) : 0;
            weekAssignment.WeekId = maxWeekId + 1;
            Weeks.Add(weekAssignment);
        }

        public int MaxUserCount
        {
            get
            {
                return Weeks.Any() ? Weeks.Max(w => w.UserCount) : 0;
            }
        }
    }
}