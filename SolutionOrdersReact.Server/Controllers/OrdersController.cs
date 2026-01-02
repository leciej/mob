using Microsoft.AspNetCore.Mvc;
using MediatR;
using SolutionOrdersReact.Dto;
using SolutionOrdersReact.Handlers.Orders;
using SolutionOrdersReact.Server.Requests.Orders.Queries;

namespace SolutionOrdersReact.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrderHandler;
    private readonly IMediator _mediator;

    public OrdersController(
        CreateOrderHandler createOrderHandler,
        IMediator mediator
    )
    {
        _createOrderHandler = createOrderHandler;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var orderId = await _createOrderHandler.Handle(request);
        return Ok(new { orderId });
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }
}
