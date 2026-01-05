using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Services.ActivityLog
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public ActivityLogService(
            ApplicationDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(
            ActivityEventType eventType,
            int? userId = null,
            string? targetType = null,
            string? targetId = null,
            string? message = null,
            object? data = null,
            CancellationToken ct = default)
        {
            var http = _httpContextAccessor.HttpContext;

            // 🔽 FALLBACK: spróbuj ustalić użytkownika (ADMIN) z HttpContext
            if (userId == null)
            {
                userId = TryResolveUserIdFromHttpContext(http);
            }

            string? dataJson = null;
            if (data != null)
            {
                dataJson = JsonSerializer.Serialize(data, JsonOptions);
            }

            var log = new Models.ActivityLog
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                CreatedAt = DateTimeOffset.UtcNow,

                UserId = userId,

                TargetType = targetType,
                TargetId = targetId,

                Message = message,
                DataJson = dataJson,

                IpAddress = http?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = http?.Request?.Headers.UserAgent.ToString(),
                Path = http?.Request?.Path.Value,
                CorrelationId = http?.TraceIdentifier
            };

            _db.ActivityLogs.Add(log);
            await _db.SaveChangesAsync(ct);
        }

        private int? TryResolveUserIdFromHttpContext(HttpContext? http)
        {
            if (http == null)
                return null;

            // 1️⃣ JWT / Claims (docelowe)
            var claim =
                http.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?? http.User?.FindFirst("sub");

            if (claim != null && int.TryParse(claim.Value, out var id))
                return id;

            // 2️⃣ Tymczasowy fallback (np. panel admina bez auth)
            if (http.Request.Headers.TryGetValue("X-Admin-Id", out var header)
                && int.TryParse(header.FirstOrDefault(), out var adminId))
            {
                return adminId;
            }

            return null;
        }
    }
}
