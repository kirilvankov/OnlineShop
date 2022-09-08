﻿namespace OnlineShop.Models.Orders
{
    public class OrderItemsViewModel
    {
        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        
    }
}
