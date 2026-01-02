using SolutionOrdersReact.Dto;
using System;
using System.Collections.Generic;

namespace SolutionOrdersReact.Dto;

public class OrderDto
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}
