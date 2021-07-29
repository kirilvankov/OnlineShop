namespace Exam_Project.Models.Products
{
    using System.Collections.Generic;

    using Exam_Project.Areas.Admin.Models.Products;

    public class ProductQueryViewModel
    {
        public string Name { get; set; }

        public string SearchTerm { get; set; }

        public Sorting Sorting { get; set; }

        public int CategoryId { get; set; }

        public int TotalItems { get; set; }

        public int ItemPerPage { get; set; }

        public IEnumerable<string> Names { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }

    }
}
