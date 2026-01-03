using System;

namespace SolutionOrdersReact.Server.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
