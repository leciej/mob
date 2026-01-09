using System;

namespace SolutionOrdersReact.Server.Models
{
    public class User
    {
        public int Id { get; set; }

        
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string Login { get; set; } = null!;

        public string? Email { get; set; }
        public string? Password { get; set; }

        
        public string Role { get; set; } = "USER";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
