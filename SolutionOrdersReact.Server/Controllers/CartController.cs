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

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddToCartRequestDto request,
            CancellationToken ct)
        {
            Console.WriteLine(">>> ADD TO CART HIT <<<");

            if (request.Quantity < 1)
                return BadRequest("Ilość musi być >= 1.");

            // =========================
            // 1️⃣ Product LUB GalleryItem
            // =========================

            string targetType;
            string name;
            decimal price;
            string? imageUrl;

            var product = await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

            if (product != null)
            {
                targetType = "Product";
                name = product.Name;
                price = product.Price;
                imageUrl = product.ImageUrl;
            }
            else
            {
                var galleryItem = await _db.GalleryItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.Id == request.ProductId, ct);

                if (galleryItem == null)
                    return NotFound("Produkt ani arcydzieło nie istnieje.");

                targetType = "GalleryItem";
                name = galleryItem.Title;
                price = galleryItem.Price;
                imageUrl = galleryItem.ImageUrl;
            }

            // =========================
            // 2️⃣ ORDER (prosty koszyk)
            // =========================

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                TotalAmount = price * request.Quantity
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);

            // =========================
            // 3️⃣ ORDER ITEM
            // =========================

            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Source = request.UserId.HasValue ? "USER" : "GUEST",
                Name = name,
                Price = price,
                Quantity = (int)request.Quantity,
                ImageUrl = imageUrl
            };

            _db.OrderItems.Add(item);
            await _db.SaveChangesAsync(ct);

            // =========================
            // 4️⃣ ACTIVITY LOG
            // =========================

            await _activityLog.LogAsync(
                ActivityEventType.CartItemAdded,
                userId: request.UserId,
                targetType: targetType,
                targetId: request.ProductId.ToString(),
                message: "Dodano do koszyka",
                data: new
                {
                    name,
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
