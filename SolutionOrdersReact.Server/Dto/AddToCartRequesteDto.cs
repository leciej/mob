using System;
using System.ComponentModel.DataAnnotations;

namespace SolutionOrdersReact.Server.Dto
{
    public class AddToCartRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        // OrderItem.Quantity jest int → DTO też int
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        // USER → wypełnione
        // GUEST → null
        public int? UserId { get; set; }
    }
}
