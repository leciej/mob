using MediatR;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Requests.Products;

public sealed record GetProductsQuery() : IRequest<List<ProductDto>>;
