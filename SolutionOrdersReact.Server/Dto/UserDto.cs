namespace SolutionOrdersReact.Server.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;

        // ✅ KLUCZOWE
        public string Role { get; set; } = null!;
    }
}
