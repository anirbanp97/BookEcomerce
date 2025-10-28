using Bookstore.API.Models;
using Bookstore.BLL.Interfaces;
using Bookstore.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetCart(int userId)
        {
            var cartItems = _cartService.GetCartByUser(userId);
            var dto = new CartDto
            {
                UserId = userId,
                Items = cartItems.Select(ci => new CartItemDto
                {
                    BookId = ci.BookId,
                    Title = ci.BookTitle ?? "",
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };

            return Ok(ApiResponse<CartDto>.Ok(dto));
        }

        [HttpPost("add")]
        public IActionResult AddToCart(int userId, int bookId, int quantity)
        {
            _cartService.AddToCart(userId, bookId, quantity);
            return Ok(ApiResponse<string>.Ok("Item added to cart."));
        }

        [HttpPut("update")]
        public IActionResult UpdateCartItem(int userId, int bookId, int quantity)
        {
            _cartService.UpdateCartItem(userId, bookId, quantity);
            return Ok(ApiResponse<string>.Ok("Cart updated."));
        }

        [HttpDelete("remove")]
        public IActionResult RemoveCartItem(int userId, int bookId)
        {
            _cartService.RemoveCartItem(userId, bookId);
            return Ok(ApiResponse<string>.Ok("Item removed from cart."));
        }
    }
}
