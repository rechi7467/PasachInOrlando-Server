using OrlandoServices.Core.DTOs;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface ICartService
    {
        List<CartItemDto> GetCart(int userId);
        void UpsertItem(int userId, UpsertCartItemDto dto);
        void RemoveItem(int userId, string cartItemId);
        void ClearCart(int userId);
        List<CartItemDto> SyncCart(int userId, List<UpsertCartItemDto> items);
    }
}
