using System;

namespace SolutionOrdersReact.Server.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public int ClientId { get; set; }

        public string AuthorName { get; set; } = null!;
        public string Text { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
