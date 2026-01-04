using System;

namespace SolutionOrdersReact.Server.Models
{
    public class GalleryRating
    {
        public Guid Id { get; set; }

        // ⬅️ POWIĄZANIE Z ARCydziełem
        public Guid GalleryItemId { get; set; }
        public GalleryItem GalleryItem { get; set; } = null!;

        // ⬅️ POWIĄZANIE Z UŻYTKOWNIKIEM
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // ⭐ 1–5
        public int Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
