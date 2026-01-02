using System;

namespace SolutionOrdersReact.Server.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string Source { get; set; } = null!;
    public Guid SourceItemId { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }
}
