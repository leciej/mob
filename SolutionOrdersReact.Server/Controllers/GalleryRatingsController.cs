using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/gallery/{galleryItemId}/ratings")]
    public class GalleryRatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GalleryRatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/gallery/{galleryItemId}/ratings
        [HttpGet]
        public async Task<ActionResult<GalleryRatingSummaryDto>> GetRatings(string galleryItemId)
        {
            // TEMP: klient = 1 (jak przy komentarzach)
            const int clientId = 1;

            var query = _context.GalleryRatings
                .AsNoTracking()
                .Where(r => r.GalleryItemId == galleryItemId);

            var votes = await query.CountAsync();
            var average = votes > 0
                ? Math.Round(await query.AverageAsync(r => r.Value), 1)
                : 0;

            var myRating = await query
                .Where(r => r.ClientId == clientId)
                .Select(r => (int?)r.Value)
                .FirstOrDefaultAsync();

            return Ok(new GalleryRatingSummaryDto
            {
                Average = average,
                Votes = votes,
                MyRating = myRating
            });
        }

        // POST /api/gallery/{galleryItemId}/ratings
        [HttpPost]
        public async Task<IActionResult> AddRating(
            string galleryItemId,
            [FromBody] CreateGalleryRatingRequest request)
        {
            if (request.Value < 1 || request.Value > 5)
                return BadRequest("Ocena musi być w zakresie 1–5.");

            // 1) sprawdź czy arcydzieło istnieje
            var existsGallery = await _context.GalleryItems
                .AsNoTracking()
                .AnyAsync(g => g.Id == galleryItemId);

            if (!existsGallery)
                return NotFound("Nie znaleziono arcydzieła.");

            // 2) sprawdź czy client istnieje
            var existsClient = await _context.Clients
                .AsNoTracking()
                .AnyAsync(c => c.IdClient == request.ClientId);

            if (!existsClient)
                return BadRequest("Nieprawidłowy klient.");

            // 3) sprawdź czy już oceniono (bez polegania na wyjątku)
            var alreadyRated = await _context.GalleryRatings
                .AsNoTracking()
                .AnyAsync(r =>
                    r.GalleryItemId == galleryItemId &&
                    r.ClientId == request.ClientId);

            if (alreadyRated)
                return Conflict("Już oceniono to arcydzieło.");

            // 4) zapis
            var rating = new GalleryRating
            {
                Id = Guid.NewGuid(),
                GalleryItemId = galleryItemId,
                ClientId = request.ClientId,
                Value = request.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.GalleryRatings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public class CreateGalleryRatingRequest
    {
        public int ClientId { get; set; }
        public int Value { get; set; }
    }

    public class GalleryRatingSummaryDto
    {
        public double Average { get; set; }
        public int Votes { get; set; }
        public int? MyRating { get; set; }
    }
}
