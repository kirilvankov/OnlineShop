namespace Exam_Project.Models.Orders
{
    using System;
    using System.Collections.Generic;

    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal TotalPrice { get; set; }

        public IEnumerable<OrderItemsViewModel> Items { get; init; }

    }
}
