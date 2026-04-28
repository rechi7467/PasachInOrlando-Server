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
        public List<OrderFieldValue> GetByOrderItemId(int orderItemId);
        List<OrderFieldValue> GetByOrderItemIdWithDetails(int orderItemId);
        public void Add(OrderFieldValue orderFieldValue);
        public void AddRange(List<OrderFieldValue> orderFieldValues);
        public void RemoveByOrderItemId(int orderItemId);
        public void SaveChanges();
    }
}
