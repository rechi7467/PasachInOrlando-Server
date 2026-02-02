using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IServiceFieldRepository
    {
        public ServiceField? GetById(int id);
        public List<ServiceField> GetByServiceId(int id);
        public void Add(ServiceField serviceField);
        public void Update(ServiceField serviceField);
        public void Remove(ServiceField serviceField);

    }
}
