using System;

namespace SolutionOrdersReact.Server.Models
{
    public class GalleryRating
    {
        public Guid Id { get; set; }

        // FK musi mieć TEN SAM TYP co GalleryItem.Id (string)
        public string GalleryItemId { get; set; } = null!;

        public int ClientId { get; set; }

        public int Value { get; set; } // 1–5

        public DateTime CreatedAt { get; set; }
    }
}
