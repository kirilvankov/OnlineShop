﻿namespace OnlineShop.Services.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }
    }
}
