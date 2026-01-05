namespace SolutionOrdersReact.Server.Dto
{
    public class ActivityLogDto
    {
        public string Id { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;

        public int? UserId { get; set; }
        public string? UserLogin { get; set; }

        public string? TargetType { get; set; }
        public string? TargetId { get; set; }

        public string? Message { get; set; }
        public string? DataJson { get; set; }
    }
}
