using System;

namespace SolutionOrdersReact.Dto;

public class OrderItemDto
{
    public string Source { get; set; } = null!;
    public Guid SourceItemId { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }
}
