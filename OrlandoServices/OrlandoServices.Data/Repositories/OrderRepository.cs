using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly DBContext _context;
        public OrderRepository(DBContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Order.ToList();
        }
        public Order GetById(int id)
        {
            var order = _context.Order.Find(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found");
            return order;
        }
        public Order CreateOrder(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
            return order;
        }
        public Order UpdateOrder(Order order,int id)
        {
            var existingOrder = _context.Order.Find(id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with id {id} not found");
            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            _context.SaveChanges();
            return existingOrder;
        }
        public void DeleteOrder(int id)
        {
            var order = _context.Order.Find(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found");
            _context.Order.Remove(order);
            _context.SaveChanges();
        }
        public List<Order> GetOrdersByUserId(int userId)
        {
            return _context.Order.Where(o => o.UserId == userId).ToList();
        }
    }
}
