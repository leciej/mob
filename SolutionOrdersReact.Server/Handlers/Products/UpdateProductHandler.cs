using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly ApplicationDbContext _db;

    public UpdateProductHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return false;

        var product = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
            return false;

        if (request.Payload.Name is not null)
            product.Name = request.Payload.Name;

        if (request.Payload.Description is not null)
            product.Description = request.Payload.Description;

        if (request.Payload.Price is not null)
            product.Price = request.Payload.Price.Value;

        if (request.Payload.ImageUrl is not null)
            product.ImageUrl = request.Payload.ImageUrl;

        product.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
