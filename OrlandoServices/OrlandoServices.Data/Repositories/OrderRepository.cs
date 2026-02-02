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

        public Order? GetById(int id)
        {
            return _context.Order.FirstOrDefault(o => o.Id == id);
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
    }
}
