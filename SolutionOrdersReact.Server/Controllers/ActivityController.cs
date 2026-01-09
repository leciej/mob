using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/activity")]
    public class ActivityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<ActivityLogDto>>> GetActivity(
            [FromQuery] int viewerUserId,
            [FromQuery] int? userId,
            [FromQuery] string? eventType,
            [FromQuery] string? targetType,
            [FromQuery] string? targetId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var viewer = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == viewerUserId);

            if (viewer == null)
                return Unauthorized("Nieprawidłowy użytkownik.");

            var isAdmin = viewer.Role == "ADMIN";

            if (!isAdmin)
            {
                if (!userId.HasValue || userId.Value != viewerUserId)
                    return Forbid("Brak dostępu do cudzej aktywności.");
            }

            var query = _context.ActivityLogs
                .AsNoTracking()
                .Include(a => a.User)
                .AsQueryable();

            if (userId.HasValue)
                query = query.Where(a => a.UserId == userId.Value);

            if (!string.IsNullOrWhiteSpace(eventType))
                query = query.Where(a => a.EventType.ToString() == eventType);

            if (!string.IsNullOrWhiteSpace(targetType))
                query = query.Where(a => a.TargetType == targetType);

            if (!string.IsNullOrWhiteSpace(targetId))
                query = query.Where(a => a.TargetId == targetId);

            query = query.OrderByDescending(a => a.CreatedAt);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ActivityLogDto
                {
                    Id = a.Id.ToString(),
                    EventType = a.EventType.ToString(),
                    CreatedAt = a.CreatedAt.ToString("O"),
                    UserId = a.UserId,
                    UserLogin = a.User != null ? a.User.Login : null,
                    TargetType = a.TargetType,
                    TargetId = a.TargetId,
                    Message = a.Message,
                    DataJson = a.DataJson
                })
                .ToListAsync();

            return Ok(new PagedResultDto<ActivityLogDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            });
        }
    }
}
