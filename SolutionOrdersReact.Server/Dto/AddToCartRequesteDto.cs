using System;
using System.ComponentModel.DataAnnotations;

namespace SolutionOrdersReact.Server.Dto
{
    public class AddToCartRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        
        
        public int? UserId { get; set; }
    }
}
