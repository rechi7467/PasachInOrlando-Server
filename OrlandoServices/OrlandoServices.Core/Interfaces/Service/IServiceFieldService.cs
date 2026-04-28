using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IServiceFieldService
    {
        public void CreateField(CreateServiceFieldDto dto);
        public void UpdateField(int fieldId, UpdateServiceFieldDto dto);
        public void DeleteField(int fieldId);
        public List<ServiceFieldClientDto> GetActiveFieldsByService(int serviceId);
        public List<ServiceFieldAdminDto> GetAllFieldsByService(int serviceId);
        ServiceFieldAdminDto GetById(int fieldId);


    }
}
