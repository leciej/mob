using System.Collections.Generic;

namespace SolutionOrdersReact.Dto;

public class CreateOrderRequest
{
    public List<OrderItemDto> Items { get; set; } = new();
}
