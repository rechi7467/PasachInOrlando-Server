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
        public OrderFieldValue? GetById(int id);
        public List<OrderFieldValue> GetByOrderId(int orderId);
        public void Add(OrderFieldValue orderFieldValue);
        public void AddRange(List<OrderFieldValue> orderFieldValues);
        public void RemoveByOrderId(int orderId);
    }
}
