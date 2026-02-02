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
        public List<AdminUser> GetAll()
        {
            return _context.AdminUser.ToList();
        }
        public AdminUser? GetById(int id)
        {
            return _context.AdminUser.Find(id);
        }
        public void Add(AdminUser adminUser)
        {
           _context.AdminUser.Add(adminUser);
        }
        public void Update(AdminUser adminUser)
        {
            _context.AdminUser.Update(adminUser);
        }
        public void Remove(AdminUser adminUser)
        {
            _context.AdminUser.Remove(adminUser);
        }
        public AdminUser? GetByName(string name)
        {
            return _context.AdminUser.FirstOrDefault(au => au.Username == name);
        }
    }
}
