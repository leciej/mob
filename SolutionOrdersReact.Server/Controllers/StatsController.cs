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

        // =========================
        // PLATFORM STATS (CARDS)
        // =========================
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

        // =========================
        // ORDERS + REVENUE (CHART)
        // =========================
        // GET /api/stats/orders-last-7-days
        [HttpGet("orders-last-7-days")]
        public async Task<ActionResult<OrdersChartDto>> GetOrdersLast7Days()
        {
            var today = DateTime.UtcNow.Date;

            var days = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-6 + i))
                .ToList();

            var ordersQuery = _db.Orders
                .Include(o => o.Items)
                .Where(o => o.CreatedAt >= days.First());

            var grouped = await ordersQuery
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Day = g.Key,
                    Orders = g.Count(),
                    Revenue = g.Sum(o =>
                        o.Items.Sum(i => i.Price * i.Quantity)
                    )
                })
                .ToListAsync();

            var result = new OrdersChartDto
            {
                Days = days
                    .Select(d => d.ToString("dd.MM"))
                    .ToList(),

                Orders = days
                    .Select(d =>
                        grouped.FirstOrDefault(g => g.Day == d)?.Orders ?? 0
                    )
                    .ToList(),

                Revenue = days
                    .Select(d =>
                        grouped.FirstOrDefault(g => g.Day == d)?.Revenue ?? 0
                    )
                    .ToList()
            };

            return Ok(result);
        }
    }

    // =========================
    // DTOs
    // =========================

    public class PlatformStatsDto
    {
        public int PurchasedCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int RatedCount { get; set; }
        public double AverageRating { get; set; }
        public int CommentsCount { get; set; }
        public int ActivitiesCount { get; set; }
    }

    public class OrdersChartDto
    {
        public List<string> Days { get; set; } = [];
        public List<int> Orders { get; set; } = [];
        public List<decimal> Revenue { get; set; } = [];
    }
}
