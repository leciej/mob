using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly ApplicationDbContext _db;

    public GetProductByIdHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return null;

        return await _db.Products
            .Where(p => p.Id == id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}
