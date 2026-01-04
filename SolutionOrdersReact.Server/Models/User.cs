using System;

namespace SolutionOrdersReact.Server.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
