using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Products;

public sealed record CreateProductCommand(
    CreateProductRequestDto Payload,
    int? ActorUserId = null
) : IRequest<ProductDto>;
