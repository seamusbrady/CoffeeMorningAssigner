using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CoffeeMorningAssigner.Models;

namespace CoffeeMorningAssigner.DAL
{
    public class UserHistoryRepository
    {
        public AssignmentHistory Read(string filename)
        {
            var repo = new CsvRepository<dynamic>();
            List<dynamic> userHistoryList = repo.Load(filename);

            if (!userHistoryList.Any())
            {
                return new AssignmentHistory();
            }

            var headers = ((IDictionary<string, object>)userHistoryList.First()).Keys.ToList();
            headers.Remove(nameof(User.Id));
            headers.Remove(nameof(User.Name));
            headers.Remove(nameof(User.Email));
            headers.Remove(nameof(User.Exclude));

            var weekAssignments = headers.ToDictionary(key => key, key => new WeekAssignment(key));

            foreach (dynamic data in userHistoryList)
            {
                var row = data as IDictionary<string, object>;
                var user = new User
                {
                    Id = int.Parse(row[nameof(User.Id)].ToString()),
                    Name = (string)row[nameof(User.Name)],
                    Email = (string)row[nameof(User.Email)]
                };

                if (bool.TryParse(row[nameof(User.Exclude)].ToString(), out var exclude))
                {
                    user.Exclude = exclude;
                }

                foreach (var key in headers)
                {
                    if (int.TryParse(row[key].ToString(), out var groupId))
                    {
                        AddUserToWeekAssignment(groupId, user, weekAssignments[key]);
                    }
                }

            }

            var history = new AssignmentHistory();
            var list = weekAssignments.ToList();
            list.Reverse();
            foreach (var item in list)
            {
                history.Add(item.Value);
            }

            return history;
        }

        private static void AddUserToWeekAssignment(int groupId, User user, WeekAssignment weekAssignment)
        {
            var group = weekAssignment.Groups.FirstOrDefault(p => p.Id == groupId);
            if (group == null)
            {
                group = new Group(groupId);
                weekAssignment.Add(group);
            }
            group.Add(user);
        }


        public void Write(string filename, List<User> users, AssignmentHistory assignmentHistory)
        {

            var weekHeaders = new Dictionary<int, string>();
            var matrix = new Dictionary<User, Dictionary<string, int>>(); // userId, weekName, groupId

            foreach (var weekAssignment in assignmentHistory.Weeks)
            {
                weekHeaders.Add(weekAssignment.WeekId, weekAssignment.Name);

                foreach (var group in weekAssignment.Groups)
                {
                    foreach (var user in group.Members)
                    {
                        if (!matrix.ContainsKey(user))
                        {
                            matrix.Add(user, new Dictionary<string, int>());
                        }

                        matrix[user].Add(weekAssignment.Name, group.Id);
                    }
                }
            }

            var records = new List<dynamic>();

            foreach (var user in users.OrderBy(u => u.Id))
            {
                dynamic record = new ExpandoObject();

                record.Id = user.Id;
                record.Name = user.Name;
                record.Email = user.Email;
                record.Exclude = user.Exclude;

                // Ensure each record has all the week Names set.
                foreach (var header in weekHeaders.OrderByDescending(w => w.Key))
                {
                    ((IDictionary<string, object>)record)[header.Value] = null;
                }

                if (matrix.ContainsKey(user))
                {
                    Dictionary<string, int> data = matrix[user];

                    foreach (var item in data.ToList())
                    {
                        ((IDictionary<string, object>)record)[item.Key] = item.Value;
                    }
                }

                records.Add(record);
            }

            new CsvRepository<dynamic>().Save(filename, records);
        }
    }
}