using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IOrderFieldValueRepository
    {
        public OrderFieldValue GetById(int id);
        public List<OrderFieldValue> GetOrderFieldValuesByOrderId(int orderId);
        public OrderFieldValue CreateOrderFieldValue(OrderFieldValue orderFieldValue);
        public List<OrderFieldValue> CreateOrderFieldValues(List<OrderFieldValue> orderFieldValues);
        public void DeleteByOrderId(int orderId);
    }
}
