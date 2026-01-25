using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IUserRepository
    {
        public List<User> GetAllUsers();
        public User GetById(int id);    
        public User CreateUser(User user);
        public User UpdateUser(User user);
        public void DeleteUser(int id);

    }
}
