using OrlandoServices.Core.Models;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IOrderItemRepository
    {
        public OrderItem? GetById(int id);
        public List<OrderItem> GetByOrderId(int orderId);
        public void Add(OrderItem orderItem);
        public void Update(OrderItem orderItem);
        public void Remove(OrderItem orderItem);
        public void SaveChanges();
    }
}
