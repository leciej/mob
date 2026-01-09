using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;

namespace SolutionOrdersReact.Server.Controllers
{
    [ApiController]
    [Route("api/gallery")]
    public class GalleryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.GalleryItems
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(items);
        }

        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _context.GalleryItems
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GalleryItemRequest dto)
        {
            var item = new GalleryItem
            {
                Id = Guid.NewGuid(),          
                Title = dto.Title,
                Artist = dto.Artist,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.GalleryItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GalleryItemRequest dto)
        {
            var existing = await _context.GalleryItems
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return NotFound();

            existing.Title = dto.Title;
            existing.Artist = dto.Artist;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.GalleryItems
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            _context.GalleryItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
