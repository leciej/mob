namespace SolutionOrdersReact.Server.Dto
{
    public class LoginUserRequestDto
    {
        public string LoginOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
