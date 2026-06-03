using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using System.Text.Json;

namespace OrlandoServices.Service
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _repo;

        public CartService(ICartItemRepository repo)
        {
            _repo = repo;
        }

        private static CartItemDto ToDto(CartItem item) => new()
        {
            CartItemId = item.CartItemId,
            ServiceId = item.ServiceId,
            ServiceName = item.ServiceName,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity,
            FieldValues = JsonSerializer.Deserialize<Dictionary<int, string>>(item.FieldValuesJson) ?? new()
        };

        private static CartItem ToEntity(int userId, UpsertCartItemDto dto) => new()
        {
            UserId = userId,
            CartItemId = dto.CartItemId,
            ServiceId = dto.ServiceId,
            ServiceName = dto.ServiceName,
            UnitPrice = dto.UnitPrice,
            Quantity = dto.Quantity,
            FieldValuesJson = JsonSerializer.Serialize(dto.FieldValues),
            CreatedAt = DateTime.UtcNow
        };

        public List<CartItemDto> GetCart(int userId) =>
            _repo.GetByUserId(userId).Select(ToDto).ToList();

        public void UpsertItem(int userId, UpsertCartItemDto dto)
        {
            var existing = _repo.GetByCartItemId(userId, dto.CartItemId);

            if (existing != null)
            {
                existing.Quantity = dto.Quantity;
                existing.ServiceName = dto.ServiceName;
                existing.UnitPrice = dto.UnitPrice;
                existing.FieldValuesJson = JsonSerializer.Serialize(dto.FieldValues);
                _repo.Update(existing);
            }
            else
            {
                _repo.Add(ToEntity(userId, dto));
            }

            _repo.SaveChanges();
        }

        public void RemoveItem(int userId, string cartItemId)
        {
            var item = _repo.GetByCartItemId(userId, cartItemId);
            if (item == null) return;
            _repo.Remove(item);
            _repo.SaveChanges();
        }

        public void ClearCart(int userId)
        {
            _repo.RemoveAllByUserId(userId);
            _repo.SaveChanges();
        }

        public List<CartItemDto> SyncCart(int userId, List<UpsertCartItemDto> items)
        {
            foreach (var dto in items)
            {
                if (_repo.GetByCartItemId(userId, dto.CartItemId) == null)
                    _repo.Add(ToEntity(userId, dto));
            }
            _repo.SaveChanges();
            return GetCart(userId);
        }
    }
}
