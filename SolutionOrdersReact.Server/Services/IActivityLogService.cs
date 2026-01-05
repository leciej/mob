using System.Threading;
using System.Threading.Tasks;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Services.ActivityLog
{
    public interface IActivityLogService
    {
        Task LogAsync(
            ActivityEventType eventType,
            int? userId = null,
            string? targetType = null,
            string? targetId = null,
            string? message = null,
            object? data = null,
            CancellationToken ct = default
        );
    }
}
