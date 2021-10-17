namespace CoffeeMorningAssigner.Models
{
    public class UserAssignment 
    {
        public UserAssignment(User user, int? groupId) 
        {
            GroupId = groupId;
            Email = user.Email;
            Name = user.Name;
            Exclude = user.Exclude;
            Id = user.Id;
        }

        public int? GroupId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public bool? Exclude { get; set; }

    }
}