using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Handlers.Orders;

public class CreateOrderHandler
{
    private readonly ApplicationDbContext _db;

    public CreateOrderHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
            Items = new List<OrderItem>()
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                // powiązanie
                Order = order,

                // uproszczony zapis pozycji zamówienia
                Source = item.Source,
                SourceItemId = item.SourceItemId,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                ImageUrl = item.ImageUrl
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return order.Id;
    }
}
