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
        // LOGIN AS GUEST
        // =========================
        [HttpPost("guest")]
        public async Task<ActionResult<UserDto>> GuestLogin()
        {
            var guest = new User
            {
                Name = "Gość",
                Surname = "",
                Login = $"GUEST_{Guid.NewGuid():N}",
                Email = null,
                Password = null,
                Role = "GUEST"
            };

            _db.Users.Add(guest);
            await _db.SaveChangesAsync();

            return Ok(Map(guest));
        }

        // =========================
        // LOGIN USER / ADMIN
        // =========================
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserRequestDto request)
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.Password != null &&
                    (u.Login == request.LoginOrEmail ||
                     u.Email == request.LoginOrEmail) &&
                    u.Password == request.Password);

            if (user == null)
                return Unauthorized();

            return Ok(Map(user));
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserRequestDto request)
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
                Password = request.Password,
                Role = "USER"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(Map(user));
        }

        // =========================
        // USER STATS  <<< TO JEST KLUCZ
        // =========================
        [HttpGet("{userId:int}/stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStats(int userId)
        {
            var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return NotFound();

            var purchasedCount = await _db.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.Items)
                .CountAsync();

            var totalSpent = await _db.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.Items)
                .SumAsync(i => i.Price * i.Quantity);

            var ratingsQuery = _db.GalleryRatings
                .Where(r => r.UserId == userId);

            var ratedCount = await ratingsQuery.CountAsync();

            var averageRating = ratedCount > 0
                ? Math.Round(await ratingsQuery.AverageAsync(r => r.Value), 1)
                : 0;

            var commentsCount = await _db.Comments
                .CountAsync(c => c.UserId == userId);

            return Ok(new UserStatsDto
            {
                PurchasedCount = purchasedCount,
                TotalSpent = totalSpent,
                RatedCount = ratedCount,
                AverageRating = averageRating,
                CommentsCount = commentsCount
            });
        }

        private static UserDto Map(User u) => new UserDto
        {
            Id = u.Id,
            Name = u.Name ?? "",
            Surname = u.Surname ?? "",
            Login = u.Login,
            Email = u.Email,
            Role = u.Role
        };
    }
}
