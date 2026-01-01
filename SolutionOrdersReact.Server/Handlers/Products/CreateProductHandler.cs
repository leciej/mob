using MediatR;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class CreateProductHandler
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly ApplicationDbContext _db;

    public CreateProductHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductDto> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Payload.Name,
            Description = request.Payload.Description,
            Price = request.Payload.Price,
            ImageUrl = request.Payload.ImageUrl,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            CreatedAt = product.CreatedAt.ToString("O"),
            UpdatedAt = product.UpdatedAt.ToString("O")
        };
    }
}
