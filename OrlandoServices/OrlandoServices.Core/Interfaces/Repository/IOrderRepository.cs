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
        public Order? GetById(int id);
        public void Add(Order order);
        public void Update(Order order);
        public void Remove(Order order);
        public List<Order> GetByUserId(int userId);
    }
}
