using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class ServiceFieldRepository : IServiceFieldRepository
    {
        private readonly DBContext _context;
        public ServiceFieldRepository(DBContext context)
        {
            _context = context;
        }
        public void Add(ServiceField serviceField)
        {
            _context.ServiceField.Add(serviceField);
        }
        public void Remove(ServiceField serviceField)
        {
            _context.ServiceField.Remove(serviceField);
        }
        public ServiceField? GetById(int id)
        {
            return _context.ServiceField.Find(id);
        }
        public List<ServiceField> GetByServiceId(int id)
        {
            return _context.ServiceField.Where(sf => sf.ServiceId == id).ToList();
        }
        public void Update(ServiceField serviceField)
        {
            _context.ServiceField.Update(serviceField);
        }

    }
}
