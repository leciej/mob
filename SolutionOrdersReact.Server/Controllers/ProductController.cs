using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Requests.Products;

namespace SolutionOrdersReact.Server.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
        => await _mediator.Send(new GetProductsQuery());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(string id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] CreateProductRequestDto payload,
        [FromQuery] int? actorUserId 
    )
    {
        var result = await _mediator.Send(
            new CreateProductCommand(payload, actorUserId));

        return Created($"/api/products/{result.Id}", result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateProductRequestDto payload,
        [FromQuery] int? actorUserId 
    )
    {
        var success = await _mediator.Send(
            new UpdateProductCommand(id, payload, actorUserId));

        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(
        string id,
        [FromQuery] int? actorUserId 
    )
    {
        var success = await _mediator.Send(
            new DeleteProductCommand(id, actorUserId));

        return success ? NoContent() : NotFound();
    }
}
