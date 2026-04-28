using Microsoft.EntityFrameworkCore;
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
        public void Add(Services service)
        {
            _context.Service.Add(service);
        }
        public void Remove(Services service)
        {
            _context.Service.Remove(service);
        }
        public List<Services> GetAll()
        {
            return _context.Service.ToList();
        }
        public List<Services> GetAllWithDetails()
        {
            return _context.Service
                .Include(s => s.ServiceFields)
                .ToList();
        }
        public Services? GetById(int id)
        {
            return _context.Service.Find(id);
        }
        public Services? GetByIdWithDetails(int id)
        {
            return _context.Service
                .Include(s => s.ServiceFields)
                .FirstOrDefault(s => s.Id == id);
        }
        public void Update(Services service)
        {
            _context.Service.Update(service);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
