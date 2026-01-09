using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly ApplicationDbContext _db;
    private readonly IActivityLogService _activityLog;

    public DeleteProductHandler(
        ApplicationDbContext db,
        IActivityLogService activityLog)
    {
        _db = db;
        _activityLog = activityLog;
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

        
        var productName = product.Name;
        var productId = product.Id.ToString();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);

        
        await _activityLog.LogAsync(
            ActivityEventType.ProductDeleted,
            userId: request.ActorUserId,
            targetType: "Product",
            targetId: productId,
            message: "Usunięto produkt",
            data: new
            {
                productName
            },
            ct: cancellationToken
        );

        return true;
    }
}
