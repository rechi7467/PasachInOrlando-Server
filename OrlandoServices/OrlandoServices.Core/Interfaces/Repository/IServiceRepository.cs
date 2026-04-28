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
        public List<Services> GetAll();
        List<Services> GetAllWithDetails();
        public Services? GetById(int id);
        Services? GetByIdWithDetails(int id);
        public void Add(Services service);
        public void Update(Services  service);
        public void Remove(Services service);
        public void SaveChanges();
    }
}
