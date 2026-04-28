using Microsoft.EntityFrameworkCore;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;

namespace OrlandoServices.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DBContext _context;

        public OrderItemRepository(DBContext context)
        {
            _context = context;
        }

        public OrderItem? GetById(int id)
        {
            return _context.OrderItem
                .Include(oi => oi.OrderFieldValues)
                .FirstOrDefault(oi => oi.Id == id);
        }

        public List<OrderItem> GetByOrderId(int orderId)
        {
            return _context.OrderItem
                .Include(oi => oi.OrderFieldValues)
                .Where(oi => oi.OrderId == orderId)
                .ToList();
        }

        public void Add(OrderItem orderItem)
        {
            _context.OrderItem.Add(orderItem);
        }

        public void Update(OrderItem orderItem)
        {
            _context.OrderItem.Update(orderItem);
        }

        public void Remove(OrderItem orderItem)
        {
            _context.OrderItem.Remove(orderItem);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
