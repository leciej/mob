using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Dto;
using SolutionOrdersReact.Server.Services.ActivityLog;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/products/{productId:guid}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogService _activityLog;

        public CommentsController(
            ApplicationDbContext context,
            IActivityLogService activityLog)
        {
            _context = context;
            _activityLog = activityLog;
        }

        // GET: api/products/{productId}/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid productId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Author = c.User.Login,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/products/{productId}/comments
        [HttpPost]
        public async Task<ActionResult<CommentDto>> AddComment(
            Guid productId,
            [FromBody] CreateCommentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Komentarz nie może być pusty.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                return BadRequest("Nieprawidłowy użytkownik.");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                UserId = user.Id,
                Text = request.Text.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // ✅ ACTIVITY LOG: CommentAdded
            await _activityLog.LogAsync(
                ActivityEventType.CommentAdded,
                userId: user.Id,
                targetType: "Product",
                targetId: productId.ToString(),
                message: "Dodano komentarz",
                data: new
                {
                    commentId = comment.Id,
                    textLength = comment.Text.Length
                }
            );

            return CreatedAtAction(
                nameof(GetComments),
                new { productId },
                new CommentDto
                {
                    Id = comment.Id,
                    Author = user.Login,
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt
                }
            );
        }
    }

    // 🔽 request do POST
    public class CreateCommentRequest
    {
        public int UserId { get; set; }
        public string Text { get; set; } = null!;
    }
}
