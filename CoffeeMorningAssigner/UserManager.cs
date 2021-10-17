using System.Collections.Generic;
using System.Linq;
using CoffeeMorningAssigner.DAL;
using CoffeeMorningAssigner.Models;

namespace CoffeeMorningAssigner
{
    public class UserManager
    {
        private readonly List<User> _users;
        
        public UserManager(string filename)
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

        public List<User> GetExcludedUsers()
        {
            return _users.Where(u => u.Exclude.GetValueOrDefault(false)).ToList();
        }

    }
}