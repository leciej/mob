namespace SolutionOrdersReact.Server.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string Login { get; set; } = null!;

        public string? Email { get; set; }

        public string Role { get; set; } = null!;
    }
}
