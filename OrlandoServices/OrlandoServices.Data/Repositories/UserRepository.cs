using Microsoft.EntityFrameworkCore;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DBContext _context;

        public UserRepository(DBContext context)
        {
            _context = context;
        }
        public User CreateUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");

            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.User.ToList();
        }

        public User GetById(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");
            return user;
        }

        public User UpdateUser(User user)
        {
            var existingUser = _context.User.Find(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with id {user.Id} not found");

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return existingUser;
        }
    }
}
