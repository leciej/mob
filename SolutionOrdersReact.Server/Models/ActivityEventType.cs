namespace SolutionOrdersReact.Server.Models
{
    public enum ActivityEventType
    {
        // Products
        ProductCreated,
        ProductUpdated,
        ProductDeleted,

        // Comments
        CommentAdded,

        // Gallery ratings
        RatingCreated,
        RatingUpdated,

        // Orders (na przyszłość)
        OrderCreated,

        // Gallery items (na przyszłość)
        GalleryItemCreated,
        GalleryItemUpdated,
        GalleryItemDeleted
    }
}
