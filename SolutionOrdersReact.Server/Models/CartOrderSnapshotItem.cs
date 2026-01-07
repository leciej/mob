using System;

namespace SolutionOrdersReact.Server.Models
{
    public class CartOrderSnapshotItem
    {
        public Guid Id { get; set; }

        public Guid CartOrderSnapshotId { get; set; }
        public CartOrderSnapshot CartOrderSnapshot { get; set; } = null!;

        // PRODUCT / GALLERY
        public string Source { get; set; } = null!;

        // Id produktu / gallery
        public Guid TargetId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }
    }
}
