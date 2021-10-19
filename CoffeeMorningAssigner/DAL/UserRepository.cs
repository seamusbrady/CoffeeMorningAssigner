using System.Collections.Generic;
using System.Linq;
using CoffeeMorningAssigner.Models;

namespace CoffeeMorningAssigner.DAL
{
    public class UserRepository
    {
        private readonly List<User> _users;
        
        public UserRepository(string filename)
        {
            _users = new CsvRepository<User>().Load(filename);
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public List<User> GetIncludedUsers()
        {
            return _users.Where(u => !u.Exclude.GetValueOrDefault(false)).ToList();
        }
    }
}