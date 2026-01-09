using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Products;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Handlers.Products;

public sealed class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly ApplicationDbContext _db;
    private readonly IActivityLogService _activityLog;

    public UpdateProductHandler(
        ApplicationDbContext db,
        IActivityLogService activityLog)
    {
        _db = db;
        _activityLog = activityLog;
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

        
        await _activityLog.LogAsync(
            ActivityEventType.ProductUpdated,
            userId: request.ActorUserId,
            targetType: "Product",
            targetId: product.Id.ToString(),
            message: "Zaktualizowano produkt",
            data: new
            {
                product.Name,
                product.Price
            },
            ct: cancellationToken
        );

        return true;
    }
}
