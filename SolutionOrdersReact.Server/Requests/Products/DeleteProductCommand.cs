using MediatR;

namespace SolutionOrdersReact.Server.Requests.Products;

public sealed record DeleteProductCommand(
    string Id,
    int? ActorUserId = null
) : IRequest<bool>;
