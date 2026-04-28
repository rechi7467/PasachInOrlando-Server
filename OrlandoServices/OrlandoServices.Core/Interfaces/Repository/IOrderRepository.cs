using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IOrderRepository
    {
        public Order? GetById(int id);
        public void Add(Order order);
        public void Update(Order order);
        public void Remove(Order order);
        public List<Order> GetByUserId(int userId);
        public List<Order> GetByStatus(OrderStatus status);
        public List<Order> GetByDateRange(DateTime from, DateTime to);
        public List<Order> GetByUserAndStatus(int userId, OrderStatus status);
        public List<Order> GetByServiceId(int serviceId);
        public List<Order> GetByStatusAndDateRange(OrderStatus status, DateTime from, DateTime to);
        public List<Order> GetAll();
        public void SaveChanges();
    }
}
