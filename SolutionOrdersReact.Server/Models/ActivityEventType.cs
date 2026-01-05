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

        // Cart
        CartItemAdded,
        CartItemQuantityChanged,
        CartItemRemoved,
        CartCleared,

        // Orders (na przyszłość / checkout)
        OrderCreated,

        // Gallery items (na przyszłość)
        GalleryItemCreated,
        GalleryItemUpdated,
        GalleryItemDeleted
    }
}
