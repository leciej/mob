using System;

namespace SolutionOrdersReact.Server.Dto
{
    public class UpdateCartItemQuantityDto
    {
        public Guid CartItemId { get; set; }
        public int Delta { get; set; } // +1 lub -1
    }
}
