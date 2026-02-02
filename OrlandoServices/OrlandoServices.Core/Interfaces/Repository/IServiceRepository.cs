using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IServiceRepository
    {
        public List<Service> GetAll();
        public Service? GetById(int id);
        public void Add(Service service);
        public void Update(Service service);
        public void Remove(Service service);

    }
}
