using System;
using System.ComponentModel.DataAnnotations;

namespace SolutionOrdersReact.Server.Models
{
    public class GalleryItem
    {
        [Key]
        public Guid Id { get; set; }   

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Artist { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
