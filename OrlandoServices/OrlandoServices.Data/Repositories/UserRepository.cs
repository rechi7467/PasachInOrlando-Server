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
        public void Add(User user)
        {
            _context.User.Add(user);
        }

        public void Remove(User user)
        {
            _context.User.Remove(user);
        }

        public List<User> GetAll()
        {
            return _context.User.ToList();
        }

        public User? GetById(int id)
        {
            return _context.User.Find(id);
        }

        public void Update(User user)
        {
            _context.User.Update(user);
        }
    }
}
