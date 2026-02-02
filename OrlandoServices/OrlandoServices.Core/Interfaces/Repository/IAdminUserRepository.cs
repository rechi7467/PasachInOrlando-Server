using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IAdminUserRepository
    {
        public List<AdminUser> GetAll();
        public AdminUser? GetById(int id);
        public AdminUser? GetByName(string name);
        public void Add(AdminUser adminUser);
        public void Update(AdminUser adminUser);
        public void Remove(AdminUser adminUser);
    }
}
