namespace SolutionOrdersReact.Server.Models
{
    public enum ActivityEventType
    {
        
        ProductCreated,
        ProductUpdated,
        ProductDeleted,

        
        CommentAdded,

        
        RatingCreated,
        RatingUpdated,

        
        CartItemAdded,
        CartItemQuantityChanged,
        CartItemRemoved,
        CartCleared,

        
        OrderCreated,

        
        GalleryItemCreated,
        GalleryItemUpdated,
        GalleryItemDeleted
    }
}
