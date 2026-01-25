using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class ServiceRepository:IServiceRepository
    {
private readonly DBContext _context;
        public ServiceRepository(DBContext context)
        {
            _context = context;
        }
        public Service CreateService(Service service)
        {
            _context.Service.Add(service);
            _context.SaveChanges();
            return service;
        }
        public void DeleteService(int id)
        {
            var service = _context.Service.Find(id);
            if (service == null)
                throw new KeyNotFoundException($"Service with id {id} not found");
            _context.Service.Remove(service);
            _context.SaveChanges();
        }
        public List<Service> GetAllServices()
        {
            return _context.Service.ToList();
        }
        public Service GetById(int id)
        {
            var service = _context.Service.Find(id);
            if (service == null)
                throw new KeyNotFoundException($"Service with id {id} not found");
            return service;
        }
        public Service UpdateService(Service service)
        {
            var existingService = _context.Service.Find(service.Id);
            if (existingService == null)
                throw new KeyNotFoundException($"Service with id {service.Id} not found");
            _context.Entry(existingService).CurrentValues.SetValues(service);
            _context.SaveChanges();
            return existingService;
        }
    }
}
