using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IOrderFieldValueService
    {
        public void CreateOrderFieldValues(int orderItemId, int serviceId, List<CreateOrderFieldValueDto> fieldValues);
        public void UpdateOrderFieldValues(int orderItemId, int serviceId, Dictionary<int, string> fieldValues);
        public List<OrderFieldValue> GetByOrderItemId(int orderItemId);
    }
}
