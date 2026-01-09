using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IActivityLogService _activityLog;

        public CheckoutController(
            ApplicationDbContext db,
            IActivityLogService activityLog)
        {
            _db = db;
            _activityLog = activityLog;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(
            [FromBody] CheckoutRequestDto request,
            CancellationToken ct)
        {
            if (request.UserId <= 0)
                return BadRequest("Nieprawidłowy użytkownik.");

            var cartItems = await _db.CartItems
                .Where(c => c.UserId == request.UserId)
                .ToListAsync(ct);

            if (cartItems.Count == 0)
                return BadRequest("Koszyk jest pusty.");

            var totalAmount = cartItems.Sum(
                c => c.Price * c.Quantity
            );

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);

            foreach (var cart in cartItems)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Source = cart.TargetType,
                    SourceItemId = cart.TargetId,
                    Name = cart.Name,
                    Price = cart.Price,
                    Quantity = cart.Quantity,
                    ImageUrl = cart.ImageUrl
                });
            }

            _db.CartItems.RemoveRange(cartItems);
            await _db.SaveChangesAsync(ct);

            await _activityLog.LogAsync(
                ActivityEventType.OrderCreated,
                userId: request.UserId,
                targetType: "Order",
                targetId: order.Id.ToString(),
                message: "Złożono zamówienie",
                data: new
                {
                    orderId = order.Id,
                    total = totalAmount,
                    items = cartItems.Count
                },
                ct: ct
            );

            return Ok(new
            {
                orderId = order.Id,
                totalAmount
            });
        }
    }

    public class CheckoutRequestDto
    {
        public int UserId { get; set; }
    }
}
