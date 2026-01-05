using System;

namespace SolutionOrdersReact.Server.Models
{
    public class ActivityLog
    {
        public Guid Id { get; set; }

        // Typ zdarzenia (enum -> string w DB)
        public ActivityEventType EventType { get; set; }

        // Kiedy się wydarzyło
        public DateTimeOffset CreatedAt { get; set; }

        // KTO wykonał akcję (opcjonalnie)
        public int? UserId { get; set; }
        public User? User { get; set; }

        // NA CZYM wykonano akcję (polimorficznie)
        // np. TargetType = "Product", TargetId = "guid"
        public string? TargetType { get; set; }
        public string? TargetId { get; set; }

        // Krótki opis (do feedu)
        public string? Message { get; set; }

        // Dowolne dane techniczne (JSON)
        public string? DataJson { get; set; }

        // Kontekst requestu (admin/debug)
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Path { get; set; }
        public string? CorrelationId { get; set; }
    }
}
