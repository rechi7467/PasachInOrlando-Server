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
        public List<Service> GetAllServices();
        public Service GetById(int id);
        public Service CreateService(Service service);
        public Service UpdateService(Service service);
        public void DeleteService(int id);

    }
}
