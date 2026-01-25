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
        public ServiceField GetById(int id);
        public List<ServiceField> GetServiceFieldsByServiceId(int id);
        public ServiceField CreateServiceField(ServiceField serviceField);
        public ServiceField UpdateServiceField(ServiceField serviceField);
        public void DeleteServiceField(int id);

    }
}
