using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class ServiceFieldRepository:IServiceFieldRepository
    {
        private readonly DBContext _context;
        public ServiceFieldRepository(DBContext context) {
                        _context = context;
        }
        public ServiceField CreateServiceField(ServiceField serviceField)
        {
            _context.ServiceField.Add(serviceField);
            _context.SaveChanges();
            return serviceField;
        }
        public void DeleteServiceField(int id)
        {
            var serviceField = _context.ServiceField.Find(id);
            if (serviceField == null)
                throw new KeyNotFoundException($"ServiceField with id {id} not found");
            _context.ServiceField.Remove(serviceField);
            _context.SaveChanges();
        }
        public ServiceField GetById(int id)
        {
            var serviceField = _context.ServiceField.Find(id);
            if (serviceField == null)
                throw new KeyNotFoundException($"ServiceField with id {id} not found");
            return serviceField;
        }
        public List<ServiceField> GetServiceFieldsByServiceId(int id)
        {
            return _context.ServiceField.Where(sf => sf.ServiceId == id).ToList();
        }
        public ServiceField UpdateServiceField(ServiceField serviceField)
        {
            var existingServiceField = _context.ServiceField.Find(serviceField.Id);
            if (existingServiceField == null)
                throw new KeyNotFoundException($"ServiceField with id {serviceField.Id} not found");
            _context.Entry(existingServiceField).CurrentValues.SetValues(serviceField);
            _context.SaveChanges();
            return existingServiceField;
        }

    }
}
