using System;

namespace SolutionOrdersReact.Server.Models
{
    public class CartSummary
    {
        // 🔑 1:1 z User
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        // 📦 suma ilości pozycji
        public int TotalItems { get; set; }

        // 💰 suma wartości koszyka
        public decimal TotalValue { get; set; }

        // 🕒 kiedy ostatnio przeliczony
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
