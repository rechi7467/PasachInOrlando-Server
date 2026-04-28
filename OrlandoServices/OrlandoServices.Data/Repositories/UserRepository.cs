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

        public List<User> GetAll()//  בלי ההזמנות והתרומת של המשתמש   
        {
            return _context.User.ToList();
        }
        public List<User> GetAllWithDetails()//  עם ההזמנות והתרומת של המשתמש
        {
            return _context.User
                .Include(u => u.Orders)
                .Include(u => u.Donations)
                .ToList();
        }

        public User? GetById(int id)//  בלי ההזמנות והתרומת של המשתמש 
        {
            return _context.User.Find(id);
        }
        public User? GetByIdWithDetails(int id)//  עם ההזמנות והתרומת של המשתמש
        {
            return _context.User
                .Include(u => u.Orders)
                .Include(u => u.Donations)
                .FirstOrDefault(u => u.Id == id);
        }
        public void Update(User user)
        {
            _context.User.Update(user);
        }
        public bool ExistsByEmail(string email)
        {
            return _context.User.Any(u => u.Email == email);
        }
        public User? GetByEmail(string email)
        {
            return _context.User.FirstOrDefault(u => u.Email == email);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
