namespace Exam_Project.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Exam_Project.Areas.Admin.Models.Products;

    public class ProductQueryViewModel
    {

        public string Name { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public Sorting Sorting { get; set; }

        [Display(Name="Category")]
        public int CategoryId { get; set; }

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; } = 2;

        public int ItemPerPage { get; set; } = 3;

        public IEnumerable<ProductViewModel> Products { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }

    }
}
