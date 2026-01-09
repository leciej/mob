using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/gallery/{galleryItemId:guid}/ratings")]
    public class GalleryRatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogService _activityLog;

        public GalleryRatingsController(
            ApplicationDbContext context,
            IActivityLogService activityLog)
        {
            _context = context;
            _activityLog = activityLog;
        }

        
        [HttpGet]
        public async Task<ActionResult<GalleryRatingSummaryDto>> GetRatings(
            Guid galleryItemId,
            [FromQuery] int? userId
        )
        {
            var ratingsQuery = _context.GalleryRatings
                .AsNoTracking()
                .Where(r =>
                    r.GalleryItemId == galleryItemId &&
                    r.UserId != null
                );

            var votes = await ratingsQuery.CountAsync();

            var average = votes > 0
                ? Math.Round(
                    await ratingsQuery.AverageAsync(r => r.Value),
                    1
                )
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

        
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRating(
            Guid galleryItemId,
            [FromBody] CreateGalleryRatingRequest request
        )
        {
            if (request.Value < 1 || request.Value > 5)
                return BadRequest("Rating value must be between 1 and 5.");

            
            var userExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == request.UserId);

            if (!userExists)
                return BadRequest("User does not exist.");

            var galleryExists = await _context.GalleryItems
                .AsNoTracking()
                .AnyAsync(g => g.Id == galleryItemId);

            if (!galleryExists)
                return NotFound("Gallery item not found.");

            var existingRating = await _context.GalleryRatings
                .FirstOrDefaultAsync(r =>
                    r.GalleryItemId == galleryItemId &&
                    r.UserId == request.UserId
                );

            var isCreate = existingRating == null;

            if (isCreate)
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
                existingRating!.Value = request.Value;
                existingRating.CreatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            
            await _activityLog.LogAsync(
                isCreate
                    ? ActivityEventType.RatingCreated
                    : ActivityEventType.RatingUpdated,
                userId: request.UserId,
                targetType: "GalleryItem",
                targetId: galleryItemId.ToString(),
                message: isCreate
                    ? "Dodano ocenę"
                    : "Zmieniono ocenę",
                data: new
                {
                    value = request.Value
                }
            );

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
