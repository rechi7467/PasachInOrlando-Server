using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Service;
using System.Security.Claims;

namespace OrlandoServices.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public IActionResult GetCart() =>
            Ok(_cartService.GetCart(GetUserId()));

        [HttpPost]
        public IActionResult UpsertItem([FromBody] UpsertCartItemDto dto)
        {
            _cartService.UpsertItem(GetUserId(), dto);
            return Ok();
        }

        [HttpDelete("{cartItemId}")]
        public IActionResult RemoveItem(string cartItemId)
        {
            _cartService.RemoveItem(GetUserId(), cartItemId);
            return Ok();
        }

        [HttpDelete]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart(GetUserId());
            return Ok();
        }

        [HttpPost("sync")]
        public IActionResult SyncCart([FromBody] List<UpsertCartItemDto> items)
        {
            var result = _cartService.SyncCart(GetUserId(), items);
            return Ok(result);
        }
    }
}
