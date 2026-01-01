using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class GetProductsHandler
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly ApplicationDbContext _db;

    public GetProductsHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Products
            .OrderByDescending(x => x.CreatedAt)
            .Select(p => new ProductDto
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt.ToString("O"),
                UpdatedAt = p.UpdatedAt.ToString("O")
            })
            .ToListAsync(cancellationToken);
    }
}
