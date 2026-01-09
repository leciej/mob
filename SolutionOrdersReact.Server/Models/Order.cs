using System;
using System.Collections.Generic;

namespace SolutionOrdersReact.Server.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        
        public Guid? ClientId { get; set; }

        
        public int? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public List<OrderItem> Items { get; set; } = new();

        
        public User? User { get; set; }
        public Client? Client { get; set; }
    }
}
