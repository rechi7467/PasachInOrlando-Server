using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IOrderService
    {
        public void CreateOrder(int userId, CreateOrderDto dto);
        public OrderResponseDto CreateGuestOrder(CreateGuestOrderDto dto);
        public void UpdateOrder(int orderId, UpdateOrderDto dto);
        public List<OrderResponseDto> GetOrdersByUser(int userId);
        public List<OrderResponseDto> GetOrdersByStatus(OrderStatus status);
        public List<OrderResponseDto> GetOrdersByDateRange(DateTime from, DateTime to);
        public List<OrderResponseDto> GetOrdersByUserAndStatus(int userId, OrderStatus status);
        public List<OrderResponseDto> GetOrdersByService(int serviceId);
        public List<OrderResponseDto> GetOrdersByStatusAndDate(OrderStatus status, DateTime from, DateTime to);
        public void UpdateOrderStatus(int orderId, OrderStatus newStatus);
        public void DeleteOrder(int orderId);
        public List<OrderResponseDto> GetAll();
    }
}
