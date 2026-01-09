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
            if (request.UserId == null)
                return BadRequest("Brak UserId.");

            if (request.ProductId == Guid.Empty)
                return BadRequest("Brak ProductId.");

            if (request.Quantity < 1)
                return BadRequest("Ilość musi być >= 1.");

            var qty = (int)request.Quantity;
            if (qty < 1)
                return BadRequest("Ilość musi być liczbą całkowitą >= 1.");

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

            var existing = await _db.CartItems.FirstOrDefaultAsync(
                c =>
                    c.UserId == request.UserId &&
                    c.TargetType == targetType &&
                    c.TargetId == request.ProductId,
                ct
            );

            if (existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId.Value,
                    TargetType = targetType,
                    TargetId = request.ProductId,
                    Name = name,
                    Price = price,
                    Quantity = qty,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync(ct);

            await _activityLog.LogAsync(
                ActivityEventType.CartItemAdded,
                userId: request.UserId,
                targetType: targetType,
                targetId: request.ProductId.ToString(),
                message: "Dodano do koszyka",
                data: new { name, quantity = qty },
                ct: ct
            );

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart(
            [FromQuery] int? userId,
            CancellationToken ct)
        {
            if (!userId.HasValue)
                return Ok(Array.Empty<object>());

            var items = await _db.CartItems
                .AsNoTracking()
                .Where(c => c.UserId == userId.Value)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    cartItemId = c.Id,
                    id = c.TargetId,
                    name = c.Name,
                    price = c.Price,
                    quantity = c.Quantity,
                    imageUrl = c.ImageUrl,
                    source = c.TargetType == "Product" ? "PRODUCTS" : "GALLERY"
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        [HttpPatch("{id:guid}/quantity")]
        public async Task<IActionResult> ChangeQuantity(
            Guid id,
            [FromQuery] int delta,
            CancellationToken ct)
        {
            if (delta == 0)
                return BadRequest("delta nie może być 0.");

            var item = await _db.CartItems.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (item == null)
                return NotFound();

            item.Quantity += delta;

            if (item.Quantity <= 0)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync(ct);

                await _activityLog.LogAsync(
                    ActivityEventType.CartItemRemoved,
                    userId: item.UserId,
                    targetType: item.TargetType,
                    targetId: item.TargetId.ToString(),
                    message: "Usunięto pozycję z koszyka",
                    data: new { delta },
                    ct: ct
                );

                return NoContent();
            }

            await _db.SaveChangesAsync(ct);

            await _activityLog.LogAsync(
                ActivityEventType.CartItemQuantityChanged,
                userId: item.UserId,
                targetType: item.TargetType,
                targetId: item.TargetId.ToString(),
                message: "Zmieniono ilość w koszyku",
                data: new { delta, newQuantity = item.Quantity },
                ct: ct
            );

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveItem(Guid id, CancellationToken ct)
        {
            var item = await _db.CartItems.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (item == null)
                return NotFound();

            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync(ct);

            await _activityLog.LogAsync(
                ActivityEventType.CartItemRemoved,
                userId: item.UserId,
                targetType: item.TargetType,
                targetId: item.TargetId.ToString(),
                message: "Usunięto pozycję z koszyka",
                data: new { cartItemId = id },
                ct: ct
            );

            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(
            [FromQuery] int? userId,
            CancellationToken ct)
        {
            if (!userId.HasValue)
                return NoContent();

            var items = _db.CartItems.Where(c => c.UserId == userId.Value);
            var removedCount = await items.CountAsync(ct);

            _db.CartItems.RemoveRange(items);
            await _db.SaveChangesAsync(ct);

            await _activityLog.LogAsync(
                ActivityEventType.CartCleared,
                userId: userId,
                targetType: "Cart",
                targetId: userId.ToString(),
                message: "Wyczyszczono koszyk",
                data: new { removedCount },
                ct: ct
            );

            return NoContent();
        }
    }
}
