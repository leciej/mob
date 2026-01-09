using System;

namespace SolutionOrdersReact.Server.Models
{
    public class ActivityLog
    {
        public Guid Id { get; set; }

        
        public ActivityEventType EventType { get; set; }

        
        public DateTimeOffset CreatedAt { get; set; }

        
        public int? UserId { get; set; }
        public User? User { get; set; }

        
        
        public string? TargetType { get; set; }
        public string? TargetId { get; set; }

        
        public string? Message { get; set; }

        
        public string? DataJson { get; set; }

        
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Path { get; set; }
        public string? CorrelationId { get; set; }
    }
}
