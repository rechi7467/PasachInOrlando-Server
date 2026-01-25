using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly DBContext _context;
        public AdminUserRepository(DBContext context)
        {
            _context = context;
        }
        public List<AdminUser> GetAllAdminUsers()
        {
            return _context.AdminUser.ToList();
        }
        public AdminUser GetAdminUserById(int id)
        {
        var existing = _context.AdminUser.Find(id);
            if (existing == null) 
                throw new KeyNotFoundException($"AdminUser with id {id} not found");
            return existing;
        }
        public AdminUser CreateAdminUser(AdminUser adminUser)
        {
            _context.AdminUser.Add(adminUser);
            _context.SaveChanges();
            return adminUser;
        }
        public AdminUser UpdateAdminUser(AdminUser adminUser,int id)
        {
            var existing = _context.AdminUser.Find(id);
            if (existing == null)
                throw new KeyNotFoundException($"AdminUser with id {id} not found");
            _context.Entry(existing).CurrentValues.SetValues(adminUser);
            _context.SaveChanges();
            return existing;
        }
        public void DeleteAdminUser(int id)
        {
            var existing = _context.AdminUser.Find(id);
            if (existing == null)
                throw new KeyNotFoundException($"AdminUser with id {id} not found");
            _context.AdminUser.Remove(existing);
            _context.SaveChanges();
        }
        public AdminUser GetAdminUserByName(string name)
        {
            var adminUser = _context.AdminUser.FirstOrDefault(au => au.Username == name);
            if (adminUser == null)
                throw new KeyNotFoundException($"AdminUser with name {name} not found");
            return adminUser;
        }
    }
}
