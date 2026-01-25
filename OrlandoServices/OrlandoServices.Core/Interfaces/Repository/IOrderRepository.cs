using OrlandoServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IOrderRepository
    {
        public List<Order> GetAllOrders();
        public Order GetById(int id);
        public Order CreateOrder(Order order);
        public Order UpdateOrder(Order order,int id);
        public void DeleteOrder(int id);
        public List<Order> GetOrdersByUserId(int userId);
    }
}
