using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Entities;
using OrderService.Services;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            order.UserId = Guid.Parse(userId);

            var orderId = await _service.CreateOrderAsync(order);

            return Ok(new
            {
                OrderId = orderId,
                Message = "Order created successfully."
            });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = await _service.GetByIdAsync(orderId);

            if (order == null)
            {
                return NotFound(new
                {
                    Message = "Order not found."
                });
            }

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var orders = await _service.GetByUserIdAsync(userId);

            return Ok(orders);
        }

        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            await _service.CancelOrderAsync(orderId);

            return Ok(new
            {
                Message = "Order cancelled successfully."
            });
        }
    }
}