using System;
using System.Collections.Generic;

namespace SolutionOrdersReact.Server.Models
{
    public class CartOrderSnapshot
    {
        public Guid Id { get; set; }

        // użytkownik, który złożył zamówienie
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // suma koszyka w momencie kliknięcia "Zamów"
        public decimal TotalValue { get; set; }

        // ile pozycji (SUMA quantity)
        public int TotalQuantity { get; set; }

        // data zatwierdzenia
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // pozycje koszyka (snapshot)
        public ICollection<CartOrderSnapshotItem> Items { get; set; }
            = new List<CartOrderSnapshotItem>();
    }
}
