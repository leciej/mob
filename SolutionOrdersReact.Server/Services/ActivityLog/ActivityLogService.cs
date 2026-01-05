using System;
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
    }
}
