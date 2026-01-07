using System;
using System.Collections.Generic;

namespace SolutionOrdersReact.Server.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        // 🏢 Klient (np. B2B / firma / legacy)
        public Guid? ClientId { get; set; }

        // 👤 Użytkownik aplikacji (USER / GUEST)
        public int? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 📦 Pozycje zamówienia
        public List<OrderItem> Items { get; set; } = new();

        // relacje
        public User? User { get; set; }
        public Client? Client { get; set; }
    }
}
