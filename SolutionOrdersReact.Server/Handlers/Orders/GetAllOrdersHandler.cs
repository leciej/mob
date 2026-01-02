using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Orders.Queries;

namespace SolutionOrdersReact.Server.Handlers.Orders;

public class GetAllOrdersHandler
    : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllOrdersHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return orders.Adapt<List<OrderDto>>();
    }
}
