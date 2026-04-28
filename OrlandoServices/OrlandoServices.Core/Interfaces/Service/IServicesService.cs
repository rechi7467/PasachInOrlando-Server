using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IServicesService
    {
        public void CreateService(CreateServiceDto dto);
        public void UpdateService(int serviceId, UpdateServiceDto dto);
        public void DeleteService(int serviceId);
        public List<ServiceClientDto> GetActiveServices();
        public ServiceClientDto GetServiceById(int id);
        public List<ServiceAdminDto> GetAllServices();
        public ServiceAdminDto GetServiceByIdForAdmin(int id);
    }
}
