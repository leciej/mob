using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IActivityLogService _activityLog;

        public CartController(
            ApplicationDbContext db,
            IActivityLogService activityLog)
        {
            _db = db;
            _activityLog = activityLog;
        }

        // USER / GUEST: AddToCart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(
    [FromBody] AddToCartRequestDto request,
    CancellationToken ct)
        {
            Console.WriteLine(">>> ADD TO CART HIT <<<");
            Console.WriteLine("REQUEST BODY:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(request));

            // reszta kodu


            // 1️⃣ sprawdź produkt
            var product = await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

            if (product == null)
                return NotFound("Produkt nie istnieje.");

            if (request.Quantity < 1)
                return BadRequest("Ilość musi być >= 1.");

            // 2️⃣ UTWÓRZ ORDER (wymagany przez FK)
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                TotalAmount = product.Price * request.Quantity
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);

            // 3️⃣ UTWÓRZ ORDER ITEM
            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Source = request.UserId.HasValue ? "USER" : "GUEST",
                Name = product.Name,
                Price = product.Price,
                Quantity = request.Quantity,
                ImageUrl = product.ImageUrl
            };

            _db.OrderItems.Add(item);
            await _db.SaveChangesAsync(ct);

            // 4️⃣ ACTIVITY LOG
            await _activityLog.LogAsync(
                ActivityEventType.CartItemAdded,
                userId: request.UserId,
                targetType: "Product",
                targetId: product.Id.ToString(),
                message: "Dodano produkt do koszyka",
                data: new
                {
                    product.Name,
                    quantity = request.Quantity,
                    orderId = order.Id
                },
                ct: ct
            );

            return Ok(new
            {
                orderId = order.Id,
                itemId = item.Id
            });
        }
    }
}
