using System.Collections.Generic;

using Exam_Project.Models.Products;

namespace Exam_Project.Areas.Admin.Models.Products
{
    public class AdminProductViewModel : ProductViewModel
    {
        public int CurrentPage { get; init; } = 1;

        public int ItemPerPage { get; init; } = 12;

        public int TotalItems { get; set; }

        public List<ProductViewModel> Products { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }
    }
}
