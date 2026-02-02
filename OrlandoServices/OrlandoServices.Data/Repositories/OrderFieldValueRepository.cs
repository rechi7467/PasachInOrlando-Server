using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class OrderFieldValueRepository : IOrderFieldValueRepository
    {
        private readonly DBContext _context;
        public OrderFieldValueRepository(DBContext context)
        {
            _context = context;
        }
        public void Add(OrderFieldValue orderFieldValue)
        {
            _context.OrderFieldValue.Add(orderFieldValue);
        }
        public OrderFieldValue? GetById(int id)
        {
            return _context.OrderFieldValue.FirstOrDefault(of => of.Id == id);
        }
        public List<OrderFieldValue> GetByOrderId(int orderId)
        {
            return _context.OrderFieldValue.Where(ofv => ofv.OrderId == orderId).ToList();
        }
        public void AddRange(List<OrderFieldValue> orderFieldValues)
        {
            _context.OrderFieldValue.AddRange(orderFieldValues);
        }
        public void RemoveByOrderId(int orderId)
        {
            var values = _context.OrderFieldValue.Where(ofv => ofv.OrderId == orderId).ToList();
            _context.OrderFieldValue.RemoveRange(values);
        }
    }
}
