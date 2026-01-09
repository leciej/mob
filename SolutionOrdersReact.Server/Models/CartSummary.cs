using System;

namespace SolutionOrdersReact.Server.Models
{
    public class CartSummary
    {
        
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        
        public int TotalItems { get; set; }

        
        public decimal TotalValue { get; set; }

        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
