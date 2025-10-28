using Bookstore.API.Models;
using Bookstore.BLL.Interfaces;
using Bookstore.Common;
using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public IActionResult PlaceOrder([FromQuery] int userId, [FromBody] OrderDto dto)
        {
            var orderItems = dto.Items.Select(i => new OrderItem
            {
                BookId = i.BookId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            var orderId = _orderService.PlaceOrder(userId, dto.ShippingAddress ?? "", orderItems);
            return Ok(ApiResponse<int>.Ok(orderId, "Order placed successfully."));
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUser(int userId)
        {
            var orders = _orderService.GetOrdersByUser(userId);
            return Ok(ApiResponse<IEnumerable<Order>>.Ok(orders));
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var order = _orderService.GetOrderDetails(orderId);
            if (order == null)
                return NotFound(ApiResponse<string>.Fail("Order not found."));

            return Ok(ApiResponse<Order>.Ok(order));
        }
    }
}
