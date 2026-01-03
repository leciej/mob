using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Dto;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/products/{productId:guid}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products/{productId}/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(Guid productId)
        {
            var comments = await _context.Comments
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Author = c.AuthorName,
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
            // 🔒 prosta walidacja
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Komentarz nie może być pusty.");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ClientId = request.ClientId,
                AuthorName = request.AuthorName,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var dto = new CommentDto
            {
                Id = comment.Id,
                Author = comment.AuthorName,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            };

            return CreatedAtAction(nameof(GetComments), new { productId }, dto);
        }
    }

    // 🔽 request do POST
    public class CreateCommentRequest
    {
        public int ClientId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
