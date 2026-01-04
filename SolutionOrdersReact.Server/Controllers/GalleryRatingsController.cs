using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/gallery/{galleryItemId:guid}/ratings")]
    public class GalleryRatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GalleryRatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/gallery/{galleryItemId}/ratings?userId=1
        [HttpGet]
        public async Task<ActionResult<GalleryRatingSummaryDto>> GetRatings(
            Guid galleryItemId,
            [FromQuery] int? userId
        )
        {
            var ratingsQuery = _context.GalleryRatings
                .AsNoTracking()
                .Where(r => r.GalleryItemId == galleryItemId && r.UserId != null); // ⬅️ ignoruj stock

            var votes = await ratingsQuery.CountAsync();

            var average = votes > 0
                ? Math.Round(await ratingsQuery.AverageAsync(r => r.Value), 1)
                : 0;

            int? myRating = null;

            if (userId.HasValue)
            {
                myRating = await ratingsQuery
                    .Where(r => r.UserId == userId.Value)
                    .Select(r => (int?)r.Value)
                    .FirstOrDefaultAsync();
            }

            return Ok(new GalleryRatingSummaryDto
            {
                Average = average,
                Votes = votes,
                MyRating = myRating
            });
        }

        // POST /api/gallery/{galleryItemId}/ratings
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRating(
            Guid galleryItemId,
            [FromBody] CreateGalleryRatingRequest request
        )
        {
            if (request.Value < 1 || request.Value > 5)
                return BadRequest();

            var galleryExists = await _context.GalleryItems
                .AsNoTracking()
                .AnyAsync(g => g.Id == galleryItemId);

            if (!galleryExists)
                return NotFound();

            var userExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == request.UserId);

            if (!userExists)
                return BadRequest();

            var existingRating = await _context.GalleryRatings
                .FirstOrDefaultAsync(r =>
                    r.GalleryItemId == galleryItemId &&
                    r.UserId == request.UserId
                );

            if (existingRating == null)
            {
                _context.GalleryRatings.Add(new GalleryRating
                {
                    Id = Guid.NewGuid(),
                    GalleryItemId = galleryItemId,
                    UserId = request.UserId,
                    Value = request.Value,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingRating.Value = request.Value;
                existingRating.CreatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class CreateGalleryRatingRequest
    {
        public int UserId { get; set; }
        public int Value { get; set; }
    }

    public class GalleryRatingSummaryDto
    {
        public double Average { get; set; }
        public int Votes { get; set; }
        public int? MyRating { get; set; }
    }
}
