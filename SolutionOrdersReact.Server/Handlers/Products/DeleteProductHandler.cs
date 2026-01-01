using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly ApplicationDbContext _db;

    public DeleteProductHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return false;

        var product = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
            return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
