using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Products;

public sealed record UpdateProductCommand(
    string Id,
    UpdateProductRequestDto Payload,
    int? ActorUserId = null
) : IRequest<bool>;
