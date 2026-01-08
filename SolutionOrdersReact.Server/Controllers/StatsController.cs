using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StatsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/stats/platform
        [HttpGet("platform")]
        public async Task<ActionResult<PlatformStatsDto>> GetPlatformStats()
        {
            var purchasedCount = await _db.OrderItems.CountAsync();

            var totalSpent = await _db.OrderItems
                .SumAsync(o => o.Price * o.Quantity);

            var ratingsQuery = _db.GalleryRatings
                .Where(r => r.UserId != null);

            var ratedCount = await ratingsQuery.CountAsync();

            var averageRating = ratedCount == 0
                ? 0
                : Math.Round(
                    await ratingsQuery.AverageAsync(r => r.Value),
                    1
                  );

            var commentsCount = await _db.Comments.CountAsync();

            // ✅ TO BYŁO BRAKUJĄCE
            var activitiesCount = await _db.ActivityLogs.CountAsync();

            return Ok(new PlatformStatsDto
            {
                PurchasedCount = purchasedCount,
                TotalSpent = totalSpent,
                RatedCount = ratedCount,
                AverageRating = averageRating,
                CommentsCount = commentsCount,
                ActivitiesCount = activitiesCount
            });
        }
    }

    public class PlatformStatsDto
    {
        public int PurchasedCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int RatedCount { get; set; }
        public double AverageRating { get; set; }
        public int CommentsCount { get; set; }

        // ✅ NOWE POLE
        public int ActivitiesCount { get; set; }
    }
}
