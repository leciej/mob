using System;

namespace SolutionOrdersReact.Server.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public int? UserId { get; set; } 

        public string TargetType { get; set; } = null!; 
        public Guid TargetId { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
