using System.Collections.Generic;

namespace CoffeeMorningAssigner.Models
{
    public class Group
    {
        public Group(int id)
        {
            Id = id;
            Members = new  List<User>();
        }

        public int Id { get; set; }
        public List<User> Members { get; }

        public bool IsMember(User user)
        {
            return Members.Contains(user);
        }

        public void Add(User user)
        {
            if (!IsMember(user))
                Members.Add(user);
        }

        public bool IsFull()
        {
            return Members.Count >= AlgorithmParameters.MaxUsersPerGroup;
        }

        public override string ToString()
        {
            return $"Group {Id}: {string.Join(", ", Members)}";
        }
    }
}