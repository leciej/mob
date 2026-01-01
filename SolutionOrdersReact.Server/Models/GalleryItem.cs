using System.ComponentModel.DataAnnotations;

namespace SolutionOrdersReact.Server.Models
{
    public class GalleryItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Artist { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
