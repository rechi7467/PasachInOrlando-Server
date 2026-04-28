using Microsoft.EntityFrameworkCore;
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
            return _context.OrderFieldValue.Find(id);
        }
        public List<OrderFieldValue> GetByOrderItemId(int orderItemId)
        {
            return _context.OrderFieldValue.Where(ofv => ofv.OrderItemId == orderItemId).ToList();
        }
        public List<OrderFieldValue> GetByOrderItemIdWithDetails(int orderItemId)
        {
            return _context.OrderFieldValue
                .Include(ofv => ofv.ServiceField)
                .Where(ofv => ofv.OrderItemId == orderItemId)
                .ToList();
        }
        public void AddRange(List<OrderFieldValue> orderFieldValues)
        {
            _context.OrderFieldValue.AddRange(orderFieldValues);
        }
        public void RemoveByOrderItemId(int orderItemId)
        {
            var values = _context.OrderFieldValue.Where(ofv => ofv.OrderItemId == orderItemId).ToList();
            _context.OrderFieldValue.RemoveRange(values);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
