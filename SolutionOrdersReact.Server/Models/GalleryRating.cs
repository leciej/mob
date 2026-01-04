using System;

namespace SolutionOrdersReact.Server.Models
{
    public class GalleryRating
    {
        public Guid Id { get; set; }

        public Guid GalleryItemId { get; set; }
        public GalleryItem GalleryItem { get; set; } = null!;

        // ⬅️ MUSI BYĆ NULLABLE (stockowe oceny)
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
