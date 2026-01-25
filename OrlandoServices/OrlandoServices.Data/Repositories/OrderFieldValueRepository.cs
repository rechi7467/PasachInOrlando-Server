using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class OrderFieldValueRepository:IOrderFieldValueRepository
    {
        private readonly DBContext _context;
        public OrderFieldValueRepository(DBContext context)
        {
            _context = context;
        }

        public OrderFieldValue CreateOrderFieldValue(OrderFieldValue orderFieldValue)
        {
            _context.OrderFieldValue.Add(orderFieldValue);
            _context.SaveChanges();
            return orderFieldValue;
        }
        public OrderFieldValue GetById(int id)
        {
            var orderFieldValue = _context.OrderFieldValue.Find(id);
            if (orderFieldValue == null)
                throw new KeyNotFoundException($"OrderFieldValue with id {id} not found");
            return orderFieldValue;
        }
        public List<OrderFieldValue> GetOrderFieldValuesByOrderId(int orderId)
        {
            var order = _context.Order.Find(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");
            return _context.OrderFieldValue.Where(ofv => ofv.OrderId == orderId).ToList();
        }
        public List<OrderFieldValue> CreateOrderFieldValues(List<OrderFieldValue> orderFieldValues)
        {
            _context.OrderFieldValue.AddRange(orderFieldValues);
            _context.SaveChanges();
            return orderFieldValues;
        }
        public void DeleteByOrderId(int orderId)
        {
            var orderFieldValues = _context.OrderFieldValue.Where(ofv => ofv.OrderId == orderId).ToList();
            if (orderFieldValues.Count == 0)
                throw new KeyNotFoundException($"No OrderFieldValues found for Order with id {orderId}");
            _context.OrderFieldValue.RemoveRange(orderFieldValues);
            _context.SaveChanges();
        }
    }
}
