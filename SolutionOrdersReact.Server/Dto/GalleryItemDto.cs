namespace SolutionOrdersReact.Server.Dto;

public sealed class GalleryItemDto
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Artist { get; set; } = default!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = default!;
}
