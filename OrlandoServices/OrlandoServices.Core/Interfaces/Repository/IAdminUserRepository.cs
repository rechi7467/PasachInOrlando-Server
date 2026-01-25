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
        public List<AdminUser> GetAllAdminUsers();
        public AdminUser GetAdminUserById(int id);
        public AdminUser GetAdminUserByName(string name);
        public AdminUser CreateAdminUser(AdminUser adminUser);
        public AdminUser UpdateAdminUser(AdminUser adminUser, int id);
        public void DeleteAdminUser(int id);
    }
}
