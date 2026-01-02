using Mapster;
using SolutionOrdersReact.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Mappings;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItemDto, OrderItem>()
            .Map(dest => dest.Source, src => src.Source)
            .Map(dest => dest.SourceItemId, src => src.SourceItemId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);

        config.NewConfig<Order, OrderDto>()
            .Map(dest => dest.Items, src => src.Items);
    }
}
