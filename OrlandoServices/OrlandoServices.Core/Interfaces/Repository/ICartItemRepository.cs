using OrlandoServices.Core.Models;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface ICartItemRepository
    {
        List<CartItem> GetByUserId(int userId);
        CartItem? GetByCartItemId(int userId, string cartItemId);
        void Add(CartItem item);
        void Update(CartItem item);
        void Remove(CartItem item);
        void RemoveAllByUserId(int userId);
        void SaveChanges();
    }
}
