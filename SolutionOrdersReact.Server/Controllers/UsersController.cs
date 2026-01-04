using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(
            RegisterUserRequestDto request)
        {
            var exists = await _db.Users.AnyAsync(u =>
                u.Login == request.Login ||
                u.Email == request.Email);

            if (exists)
                return BadRequest("Login lub email już istnieje");

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Login = request.Login,
                Email = request.Email,
                Password = request.Password
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Login = user.Login,
                Email = user.Email
            };
        }

        // =========================
        // LOGIN (LOGIN LUB EMAIL)
        // =========================
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(
            LoginUserRequestDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                (u.Login == request.LoginOrEmail ||
                 u.Email == request.LoginOrEmail)
                && u.Password == request.Password);

            if (user == null)
                return Unauthorized();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Login = user.Login,
                Email = user.Email
            };
        }
    }
}
