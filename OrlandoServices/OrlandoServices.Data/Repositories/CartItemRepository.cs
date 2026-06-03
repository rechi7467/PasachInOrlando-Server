using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;

namespace OrlandoServices.Data.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DBContext _context;

        public CartItemRepository(DBContext context)
        {
            _context = context;
        }

        public List<CartItem> GetByUserId(int userId)
        {             
           return _context.CartItem.Where(c => c.UserId == userId).ToList();
        }
        public CartItem? GetByCartItemId(int userId, string cartItemId)
        { 
           return _context.CartItem.FirstOrDefault(c => c.UserId == userId && c.CartItemId == cartItemId);
        }
        public void Add(CartItem item)
        {
         _context.CartItem.Add(item);
        }
        public void Update(CartItem item)
        {
         _context.CartItem.Update(item);
        }
        public void Remove(CartItem item)
        {
         _context.CartItem.Remove(item);
        }
        public void RemoveAllByUserId(int userId)
        {
            var items = _context.CartItem.Where(c => c.UserId == userId).ToList();
            _context.CartItem.RemoveRange(items);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }       
    }
}
