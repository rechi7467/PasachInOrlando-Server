using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderFieldValueService _orderFieldValueService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IUserRepository userRepository,
            IServiceRepository serviceRepository,
            IOrderFieldValueService orderFieldValueService,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
            _orderFieldValueService = orderFieldValueService;
            _unitOfWork = unitOfWork;
        }

        private static OrderItemResponseDto ToItemResponseDto(OrderItem item) => new OrderItemResponseDto
        {
            Id = item.Id,
            ServiceId = item.ServiceId,
            ServiceName = item.ServiceName,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity,
            DiscountAmount = item.DiscountAmount,
            TotalPrice = item.TotalPrice,
            FieldValues = item.OrderFieldValues.Select(fv => new OrderFieldValueResponseDto
            {
                ServiceFieldId = fv.ServiceFieldId,
                FieldName = fv.FieldNameAtOrderTime,
                Value = fv.Value
            }).ToList()
        };

        private OrderResponseDto ToResponseDto(Order order)
        {
            var items = _orderItemRepository.GetByOrderId(order.Id);
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CustomerFirstName = order.User?.FirstName,
                CustomerLastName = order.User?.LastName,
                CustomerEmail = order.User?.Email,
                CustomerPhone = order.User?.Phone,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Notes = order.Notes,
                Items = items.Select(ToItemResponseDto).ToList()
            };
        }

        // פונקציה פנימית המבנה הזמנה שלמה: הזמנה ראשית + פריטים + ערכי שדות דינמיים
        // - שומר את ההזמנה תחילה (TotalAmount=0) כדי לקבל Id מה-DB
        // - לכל פריט: מוודא שהשירות קיים ופעיל, שולף מחיר מה-DB (לא מהלקוח)
        // - שומר snapshot של שם השירות ומחיר בזמן ההזמנה — כך גם אם השירות ישתנה, ההזמנה תישאר נכונה
        // - בסוף מחשב ומעדכן TotalAmount האמיתי לפי כל הפריטים
        private Order BuildOrder(int userId, List<CreateOrderItemDto> items)
        {
            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0
            };

            _orderRepository.Add(order);
            _orderRepository.SaveChanges();

            decimal totalAmount = 0;

            foreach (var itemDto in items)
            {
                if (itemDto.ServiceId <= 0)
                    throw new ArgumentException("Invalid ServiceId in order item");

                if (itemDto.Quantity <= 0)
                    throw new ArgumentException("Quantity must be at least 1");

                var service = _serviceRepository.GetById(itemDto.ServiceId);
                if (service == null || service.Status != ServiceStatus.Active)
                    throw new NotFoundException($"Service {itemDto.ServiceId} not found or unavailable");

                var totalPrice = service.BasePrice * itemDto.Quantity;

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ServiceId = service.Id,
                    ServiceName = service.Name,
                    UnitPrice = service.BasePrice,
                    Quantity = itemDto.Quantity,
                    DiscountAmount = 0,
                    TotalPrice = totalPrice
                };

                _orderItemRepository.Add(orderItem);
                _orderItemRepository.SaveChanges();

                if (itemDto.FieldValues != null && itemDto.FieldValues.Any())
                {
                    var fieldValueDtos = itemDto.FieldValues
                        .Select(kv => new CreateOrderFieldValueDto
                        {
                            ServiceFieldId = kv.Key,
                            Value = kv.Value
                        }).ToList();

                    _orderFieldValueService.CreateOrderFieldValues(orderItem.Id, service.Id, fieldValueDtos);
                }

                totalAmount += totalPrice;
            }

            order.TotalAmount = totalAmount;
            _orderRepository.Update(order);
            _orderRepository.SaveChanges();

            return order;
        }

        // יצירת הזמנה עבור משתמש מחובר בתוך Transaction
        // - מוודא שהמשתמש קיים ופעיל לפני שמתחיל כלום
        // - Transaction: אם כל שלב הצליח → Commit, אם משהו נכשל → Rollback מלא
        // - מעביר ל-BuildOrder שמטפל בכל הלוגיקה של הפריטים
        public OrderResponseDto CreateOrder(int userId, CreateOrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Order must have at least one item");

            var user = _userRepository.GetById(userId);
            if (user == null || !user.IsActive)
                throw new ArgumentException("Invalid user");

            _unitOfWork.BeginTransaction();
            try
            {
                var order = BuildOrder(userId, dto.Items);
                _unitOfWork.Commit();
                return ToResponseDto(order);
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        // יצירת הזמנה לאורח — יוצר משתמש חדש (ללא סיסמה) ואת ההזמנה בTransaction אחד
        // - בודק אם האימייל כבר קיים: אם כן — משתמש בו; אם לא — יוצר משתמש חדש
        // - המשתמש שנוצר אין לו PasswordHash — הוא "אורח"; יוכל להגדיר סיסמה אחר כך דרך SetPassword
        // - כל הפעולה בTransaction: אם ההזמנה נכשלת — גם יצירת המשתמש מתבטלת
        public OrderResponseDto CreateGuestOrder(CreateGuestOrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Order must have at least one item");

            _unitOfWork.BeginTransaction();
            try
            {
                var user = _userRepository.GetByEmail(dto.Email.Trim());

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        Email = dto.Email.Trim(),
                        Phone = dto.Phone.Trim(),
                        BillingAddress = dto.BillingAddress.Trim(),
                        City = dto.City.Trim(),
                        State = dto.State.Trim(),
                        ZipCode = dto.ZipCode.Trim(),
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _userRepository.Add(user);
                    _userRepository.SaveChanges();
                }
                else if (!user.IsActive)
                {
                    throw new ArgumentException("This account is inactive. Please contact support.");
                }

                var order = BuildOrder(user.Id, dto.Items);
                _unitOfWork.Commit();

                return ToResponseDto(order);
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void UpdateOrder(int orderId, UpdateOrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var order = _orderRepository.GetById(orderId);
            if (order == null)
                throw new NotFoundException($"Order {orderId} not found");

            var user = _userRepository.GetById(order.UserId);
            if (user == null || !user.IsActive)
                throw new ArgumentException("Invalid user for this order");

            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be updated");

            _unitOfWork.BeginTransaction();
            try
            {
                if (dto.ItemsToAdd != null && dto.ItemsToAdd.Any())
                {
                    foreach (var itemDto in dto.ItemsToAdd)
                    {
                        var service = _serviceRepository.GetById(itemDto.ServiceId);
                        if (service == null || service.Status != ServiceStatus.Active)
                            throw new NotFoundException($"Service {itemDto.ServiceId} not found or unavailable");

                        var totalPrice = service.BasePrice * itemDto.Quantity;

                        var orderItem = new OrderItem
                        {
                            OrderId = orderId,
                            ServiceId = service.Id,
                            ServiceName = service.Name,
                            UnitPrice = service.BasePrice,
                            Quantity = itemDto.Quantity,
                            DiscountAmount = 0,
                            TotalPrice = totalPrice
                        };

                        _orderItemRepository.Add(orderItem);
                        _orderItemRepository.SaveChanges();

                        if (itemDto.FieldValues != null && itemDto.FieldValues.Any())
                        {
                            var fieldValueDtos = itemDto.FieldValues
                                .Select(kv => new CreateOrderFieldValueDto
                                {
                                    ServiceFieldId = kv.Key,
                                    Value = kv.Value
                                }).ToList();

                            _orderFieldValueService.CreateOrderFieldValues(orderItem.Id, service.Id, fieldValueDtos);
                        }
                    }
                }

                if (dto.ItemIdsToRemove != null && dto.ItemIdsToRemove.Any())
                {
                    foreach (var itemId in dto.ItemIdsToRemove)
                    {
                        var item = _orderItemRepository.GetById(itemId);
                        if (item == null || item.OrderId != orderId)
                            throw new NotFoundException($"Order item {itemId} not found in this order");

                        _orderItemRepository.Remove(item);
                        _orderItemRepository.SaveChanges();
                    }
                }

                if (dto.ItemsToUpdate != null && dto.ItemsToUpdate.Any())
                {
                    foreach (var kvp in dto.ItemsToUpdate)
                    {
                        var item = _orderItemRepository.GetById(kvp.Key);
                        if (item == null || item.OrderId != orderId)
                            throw new NotFoundException($"Order item {kvp.Key} not found in this order");

                        var updateDto = kvp.Value;

                        if (updateDto.Quantity.HasValue)
                        {
                            if (updateDto.Quantity.Value <= 0)
                                throw new ArgumentException("Quantity must be at least 1");

                            item.Quantity = updateDto.Quantity.Value;
                            item.TotalPrice = (item.UnitPrice * item.Quantity) - item.DiscountAmount;
                        }

                        _orderItemRepository.Update(item);
                        _orderItemRepository.SaveChanges();

                        if (updateDto.FieldValues != null && updateDto.FieldValues.Any())
                            _orderFieldValueService.UpdateOrderFieldValues(item.Id, item.ServiceId, updateDto.FieldValues);
                    }
                }

                var allItems = _orderItemRepository.GetByOrderId(orderId);
                order.TotalAmount = allItems.Sum(i => i.TotalPrice);
                order.UpdatedAt = DateTime.UtcNow;
                _orderRepository.Update(order);
                _orderRepository.SaveChanges();

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot change cancelled order");

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepository.Update(order);
            _orderRepository.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be cancelled");

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepository.Update(order);
            _orderRepository.SaveChanges();
        }

        public List<OrderResponseDto> GetOrdersByUser(int userId) =>
            _orderRepository.GetByUserId(userId).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetOrdersByStatus(OrderStatus status) =>
            _orderRepository.GetByStatus(status).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetOrdersByDateRange(DateTime from, DateTime to) =>
            _orderRepository.GetByDateRange(from, to).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetOrdersByUserAndStatus(int userId, OrderStatus status) =>
            _orderRepository.GetByUserAndStatus(userId, status).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetOrdersByService(int serviceId) =>
            _orderRepository.GetByServiceId(serviceId).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetOrdersByStatusAndDate(OrderStatus status, DateTime from, DateTime to) =>
            _orderRepository.GetByStatusAndDateRange(status, from, to).Select(ToResponseDto).ToList();

        public List<OrderResponseDto> GetAll() =>
            _orderRepository.GetAll().Select(ToResponseDto).ToList();
    }
}
