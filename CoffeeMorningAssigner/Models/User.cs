using System;

namespace CoffeeMorningAssigner.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Email { set; get; }
        public bool? Exclude { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as User;
            return Equals(item);
        }

        protected bool Equals(User other)
        {
            return string.Equals(Name, other.Name) &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Email.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            //return $"{Email}";
            return $"{Id}:{Name}";
        }
    }
}