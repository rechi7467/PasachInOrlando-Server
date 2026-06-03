using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models.Enums;
using System.Security.Claims;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // מחובר — יצירת הזמנה, userId נשלף מהטוקן
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            try
            {
                var order = _orderService.CreateOrder(int.Parse(userIdClaim), dto);
                return Ok(order);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אורח — יצירת הזמנה עם פרטים אישיים, מחזיר userId ליצירת סיסמה אחר כך
        [HttpPost("guest")]
        public IActionResult CreateGuest([FromBody] CreateGuestOrderDto dto)
        {
            try
            {
                var result = _orderService.CreateGuestOrder(dto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // מחובר — הזמנות של המשתמש, userId נשלף מהטוקן
        [Authorize]
        [HttpGet("my")]
        public IActionResult GetMyOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var orders = _orderService.GetOrdersByUser(int.Parse(userIdClaim));
            return Ok(orders);
        }

        // מחובר — עדכון הזמנה (רק אם ממתינה)
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateOrderDto dto)
        {
            try
            {
                _orderService.UpdateOrder(id, dto);
                return Ok("Order updated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // מחובר — ביטול הזמנה (רק אם ממתינה)
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return Ok("Order cancelled successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אדמין — שינוי סטטוס הזמנה
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] OrderStatus newStatus)
        {
            try
            {
                _orderService.UpdateOrderStatus(id, newStatus);
                return Ok("Order status updated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אדמין — כל ההזמנות עם פילטרים אופציונליים
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] OrderStatus? status,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? serviceId,
            [FromQuery] int? userId)
        {
            if (serviceId.HasValue)
                return Ok(_orderService.GetOrdersByService(serviceId.Value));

            if (userId.HasValue && status.HasValue)
                return Ok(_orderService.GetOrdersByUserAndStatus(userId.Value, status.Value));

            if (userId.HasValue)
                return Ok(_orderService.GetOrdersByUser(userId.Value));

            if (status.HasValue && from.HasValue && to.HasValue)
                return Ok(_orderService.GetOrdersByStatusAndDate(status.Value, from.Value, to.Value));

            if (from.HasValue && to.HasValue)
                return Ok(_orderService.GetOrdersByDateRange(from.Value, to.Value));

            if (status.HasValue)
                return Ok(_orderService.GetOrdersByStatus(status.Value));

            return Ok(_orderService.GetAll());
        }
    }
}
