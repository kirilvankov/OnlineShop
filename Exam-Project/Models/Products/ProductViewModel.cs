namespace Exam_Project.Models.Products
{
   
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
           
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int? OrderingNumber { get; set; }

        public int CategoryId { get; set; }
    }
}
