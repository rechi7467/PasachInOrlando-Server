using Microsoft.EntityFrameworkCore;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DBContext _context;
        public OrderRepository(DBContext context)
        {
            _context = context;
        }

        public Order? GetById(int id)
        {
            return _context.Order.Find(id);
        }
        public void Add(Order order)
        {
            _context.Order.Add(order);
        }
        public void Update(Order order)
        {
            _context.Order.Update(order);
        }
        public void Remove(Order order)
        {
            _context.Order.Remove(order);
        }
        public List<Order> GetByUserId(int userId)
        {
            return _context.Order.Where(o => o.UserId == userId).ToList();
        }
        public List<Order> GetByStatus(OrderStatus status)
        {
            return _context.Order.Where(o => o.Status == status).ToList();
        }
        public List<Order> GetByDateRange(DateTime from, DateTime to)
        {
            return _context.Order.Where(o => o.CreatedAt >= from && o.CreatedAt <= to).ToList();
        }
        public List<Order> GetByUserAndStatus(int userId, OrderStatus status)
        {
            return _context.Order.Where(o => o.UserId == userId && o.Status == status).ToList();
        }
        public List<Order> GetByServiceId(int serviceId)
        {
            return _context.Order.Where(o => o.OrderItems.Any(oi => oi.ServiceId == serviceId)).ToList();
        }
        public List<Order> GetByStatusAndDateRange(OrderStatus status,DateTime from,DateTime to)
        {
            return _context.Order.Where(o =>o.Status == status &&o.CreatedAt >= from &&o.CreatedAt <= to).ToList();
        }

        public List<Order> GetAll()
        {
            return _context.Order.Include(o => o.User).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
