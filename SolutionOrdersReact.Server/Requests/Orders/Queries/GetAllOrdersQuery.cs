using MediatR;
using SolutionOrdersReact.Dto;
using System.Collections.Generic;

namespace SolutionOrdersReact.Server.Requests.Orders.Queries;

public class GetAllOrdersQuery : IRequest<List<OrderDto>>
{
}
