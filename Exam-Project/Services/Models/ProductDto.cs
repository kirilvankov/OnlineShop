namespace Exam_Project.Services.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int? OrderingNumber { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
    }
}
