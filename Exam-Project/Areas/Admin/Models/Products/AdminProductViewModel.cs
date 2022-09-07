using System.Collections.Generic;

using Exam_Project.Models.Products;

namespace Exam_Project.Areas.Admin.Models.Products
{
    public class AdminProductViewModel : ProductViewModel
    {
        public int PageIndex { get; init; } = 1;

        public int PageSize { get; init; } = 3;

        public int TotalItems { get; set; }

        public List<ProductViewModel> Products { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }
    }
}
