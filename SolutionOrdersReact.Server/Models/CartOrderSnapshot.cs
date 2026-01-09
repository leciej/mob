using System;
using System.Collections.Generic;

namespace SolutionOrdersReact.Server.Models
{
    public class CartOrderSnapshot
    {
        public Guid Id { get; set; }

        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        
        public decimal TotalValue { get; set; }

        
        public int TotalQuantity { get; set; }

        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public ICollection<CartOrderSnapshotItem> Items { get; set; }
            = new List<CartOrderSnapshotItem>();
    }
}
