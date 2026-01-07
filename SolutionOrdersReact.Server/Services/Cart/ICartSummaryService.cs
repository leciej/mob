using System.Threading;
using System.Threading.Tasks;

namespace SolutionOrdersReact.Server.Services.Cart
{
    public interface ICartSummaryService
    {
        Task RecalculateAsync(int userId, CancellationToken ct = default);
        Task ClearAsync(int userId, CancellationToken ct = default);
    }
}
