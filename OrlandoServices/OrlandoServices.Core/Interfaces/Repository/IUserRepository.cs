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
        public List<User> GetAll();
        public User? GetById(int id);    
        public void Add(User user);
        public void Update(User user);
        public void Remove(User user);

    }
}
