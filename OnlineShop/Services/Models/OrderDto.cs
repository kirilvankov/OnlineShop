namespace OnlineShop.Services.Models
{
    using System.Collections.Generic;
    using System;

    public class OrderDto
    {
        public DateTime CreatedAt { get; set; }

        public string UserId { get; init; }

        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
