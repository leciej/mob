using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Requests.Items.Queries;

namespace SolutionOrdersReact.Server.Handlers.Items
{
    public class GetAllItemsHandler(ApplicationDbContext _context) : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(
            GetAllItemsQuery request,
            CancellationToken cancellationToken)
        {
            
            var items = await _context.Items
                .Include(i => i.Category)              
                .Include(i => i.UnitOfMeasurement)     
                .Where(i => i.IsActive)                
                .OrderBy(i => i.Name)                  
                .Select(i => new ItemDto               
                {
                    IdItem = i.IdItem,
                    Name = i.Name,
                    Description = i.Description,
                    IdCategory = i.IdCategory,
                    CategoryName = i.Category.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    IdUnitOfMeasurement = i.IdUnitOfMeasurement,
                    UnitName = i.UnitOfMeasurement != null
                        ? i.UnitOfMeasurement.Name
                        : null,
                    Code = i.Code,
                    IsActive = i.IsActive
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}