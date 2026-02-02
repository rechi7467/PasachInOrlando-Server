using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DBContext _context;
        public ServiceRepository(DBContext context)
        {
            _context = context;
        }
        public void Add(Service service)
        {
            _context.Service.Add(service);
        }
        public void Remove(Service service)
        {
            _context.Service.Remove(service);
        }
        public List<Service> GetAll()
        {
            return _context.Service.ToList();
        }
        public Service? GetById(int id)
        {
            return _context.Service.FirstOrDefault(s => s.Id == id);
        }
        public void Update(Service service)
        {
            _context.Service.Update(service);
        }
    }
}
